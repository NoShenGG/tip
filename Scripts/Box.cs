using Godot;
using Tip.Scripts.TimeMechanics;

public partial class Box : TimeObject
{
	[ExportCategory("Spawning")]
	[Export] private Vector3 _spawnPos = Vector3.Zero;

	[ExportCategory("Pickup")]
	[Export] private float _trackModifier = 20.0f;

	private Node3D _pickUpPos;

	public override void _Ready() {
		ProcessMode = Node.ProcessModeEnum.Pausable;
		base._Ready();
		Position = _spawnPos;
	}

	public void Respawn() {
		Position = _spawnPos;
		Rotation = Vector3.Zero;
		Freeze = true;
		_objectHistory.Clear();
		Freeze = false;
	}

	public override void _PhysicsProcess(double delta) {
		if (_pickUpPos != null) {
			Vector3 dir = GlobalPosition.DirectionTo(_pickUpPos.GlobalPosition).Normalized();
			float mag = GlobalPosition.DistanceTo(_pickUpPos.GlobalPosition);

			LinearVelocity = dir * mag * _trackModifier;
		}
		base._PhysicsProcess(delta);
	}

	public void SetPickupPos(Node3D pickUpPos) {
		_pickUpPos = pickUpPos;
	}
}
