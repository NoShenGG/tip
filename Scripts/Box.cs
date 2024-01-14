using Godot;
using Tip.Scripts.TimeMechanics;

public partial class Box : TimeObject
{
	[ExportCategory("Spawning")]
	[Export] private Vector3 _spawnPos = Vector3.Zero;

	public override void _Ready() {
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
}
