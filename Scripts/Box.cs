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
		if (IsInstanceValid(_pickUpPos)) {
			Vector3 dir = GlobalPosition.DirectionTo(_pickUpPos.GlobalPosition).Normalized();
			float mag = GlobalPosition.DistanceTo(_pickUpPos.GlobalPosition);

			LinearVelocity = dir * mag * _trackModifier;
			Rotation = _pickUpPos.GlobalRotation;
			Rotation = new Vector3(0, Rotation.Y, Rotation.Z);
		}
		base._PhysicsProcess(delta);
	}

	public void SetPickupPos(Node3D pickUpPos) {
		_pickUpPos = pickUpPos;
	}
}
