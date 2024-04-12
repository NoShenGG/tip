using Godot;

namespace Tip.Scripts;

public partial class DeathPlane : Area3D {
	[Export] private Vector3 _playerSpawnPos = Vector3.Zero;
	[Export] private Vector3 _playerSpawnRot = Vector3.Zero;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		BodyEntered += KillEntity;	
	}

	private void KillEntity(Node3D entity) {
		if (entity is Box box) {
			box.Respawn();
		} else if (entity is Player player) {
			player.Position = _playerSpawnPos;
			player.Rotation = _playerSpawnRot;
			player.Velocity = Vector3.Zero;
		}
	}
}