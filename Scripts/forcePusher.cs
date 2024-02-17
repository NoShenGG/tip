using System;
using System.Diagnostics.Tracing;
using Godot;


namespace Tip.Scripts;
public partial class forcePusher : Area3D
{
	[ExportCategory("Pusher Settings")]
	//Static for consistency across all pushers
	[Export] private float pusherForce = 0.3f;
	//Gives a boost to bodies that enter the area, allows for hovering
	[Export] private float entryBoost = 2.5f;
    // Called when the node enters the scene tree for the first time.
	[Export] private bool isPuller = false;
	private Godot.Collections.Array<Godot.Node3D> bodies;

    public override void _Ready()
    {
        base._Ready();
		//If the object is a puller, sets all variables as negative
		if (isPuller) {
			pusherForce = -pusherForce;
			entryBoost = -entryBoost;
		}
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		bodies = GetOverlappingBodies();
		foreach (Node3D node in bodies) {
			if (node is Player player) {
				//Adds a basis vector local to the rotation of the pusher to the player's velocity, effectively pushing them in the direction of the pusher
				player.Velocity = player.Velocity + (Transform.Basis.Y * pusherForce);
				} else if (node is RigidBody3D body) {
					body.LinearVelocity = body.LinearVelocity + (Transform.Basis.Y * pusherForce);
				}
		}
    }

	public void _OnForcePusherBodyEntered(Node3D node) {
		if (node is Player player) {
			player.Velocity = player.Velocity + (Transform.Basis.Y * entryBoost);
		} else if (node is RigidBody3D body) {
			body.LinearVelocity = body.LinearVelocity + (Transform.Basis.Y * entryBoost);
		}
	}
}
