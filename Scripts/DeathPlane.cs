using Godot;

namespace Tip.Scripts;

public partial class DeathPlane : Area3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		BodyEntered += KillEntity;
	}

	private void KillEntity(Node3D entity) {
		if (entity is Box box) {
			box.Respawn();
		} else if (entity is Player player) {
			player.GlobalPosition = new Vector3(0, 1, -7);
			player.GlobalRotation = new Vector3(0, Mathf.DegToRad(-90), 0);
		}
	}
}