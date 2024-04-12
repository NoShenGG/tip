using Godot;
using Tip.Scripts.TimeMechanics;

namespace Tip.Scripts;

public partial class Box : TimeObject
{
	[ExportCategory("Spawning")]
	[Export] private Vector3 _spawnPos = Vector3.Zero;

	public bool EnableTimeBehavior;

	public override void _Ready() {
		base._Ready();
		Position = _spawnPos;
		EnableTimeBehavior = true;
	}

	public void Respawn() {
		Position = _spawnPos;
		Rotation = Vector3.Zero;
		Freeze = true;
		Keyframes.Clear();
		if (_currentTimeState == TimeState.Normal) {
			Freeze = false;
		}
	}

	public override void _PhysicsProcess(double delta) {
		if (EnableTimeBehavior) {
			base._PhysicsProcess(delta);
		}
	}
}
