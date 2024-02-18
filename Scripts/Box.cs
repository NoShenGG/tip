using Godot;
using Tip.Scripts.TimeMechanics;

namespace Tip.Scripts;

public partial class Box : TimeObject
{
	[ExportCategory("Spawning")]
	[Export] private Vector3 _spawnPos = Vector3.Zero;

	[ExportCategory("Pickup")]
	[Export] private float _trackModifier = 20.0f;

	private Node3D _pickUpPos;
	private bool _handlePickup;

	public override void _Ready() {
		base._Ready();
		Position = _spawnPos;
	}

	public void Respawn() {
		Position = _spawnPos;
		Rotation = Vector3.Zero;
		Freeze = true;
		Keyframes.Clear();
		Freeze = false;
	}

	public override void _PhysicsProcess(double delta) {
		if (IsInstanceValid(_pickUpPos) && _currentTimeState != TimeState.Rewinding) {
			Freeze = false;
			Vector3 dir = GlobalPosition.DirectionTo(_pickUpPos.GlobalPosition).Normalized();
			float mag = GlobalPosition.DistanceTo(_pickUpPos.GlobalPosition);

			if (mag > 1) {
				_pickUpPos = null;
			} else {
				LinearVelocity = dir * mag * _trackModifier;
				Rotation = _pickUpPos.GlobalRotation;
				Rotation = new Vector3(0, Rotation.Y, Rotation.Z);
			}
			if (_currentTimeState == TimeState.Normal) {
				base._PhysicsProcess(delta);
			}
		} else if (_currentTimeState == TimeState.Stopped) {
			Freeze = true;
			_pickUpPos = null;
			base._PhysicsProcess(delta);
		} else {
			_pickUpPos = null;
			base._PhysicsProcess(delta);
		}
	}

	public void SetPickupPos(Node3D pickUpPos) {
		_pickUpPos = pickUpPos;
	}
}
