using System.Diagnostics.Tracing;
using Godot;


namespace Tip.Scripts;
public partial class forcePusher : Area3D
{
	[ExportCategory("Pusher Settings")]
	//Static for consistency across all pushers
	[Export] private float pusherForce = 0.3f;
	[Export] private float entryBoost = 2.5f;
    // Called when the node enters the scene tree for the first time.
	private Godot.Collections.Array<Godot.Node3D> bodies;
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		bodies = GetOverlappingBodies();
		foreach (Node3D node in bodies) {
			if (node is Player player) {
				//Adds a basis vector local to the rotation of the pusher to the player's velocity, effectively pushing them in the direction of the pusher
				player.Velocity = player.Velocity + (Transform.Basis.Y * pusherForce);
				} else if (node is RigidBody3D body) {
					body.ApplyCentralImpulse(Transform.Basis.Y * pusherForce);
				}
		}
    }

	public void _OnForcePusherBodyEntered(Node3D node) {
		if (node is Player player) {
			player.Velocity = new Vector3(player.Velocity.X, player.Velocity.Y + entryBoost, player.Velocity.Z);
		} else if (node is RigidBody3D body) {
			body.LinearVelocity = new Vector3(body.LinearVelocity.X, body.LinearVelocity.Y + entryBoost, body.LinearVelocity.Z);
		}
	}
}
