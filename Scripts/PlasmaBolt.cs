using Godot;
using System;
using Tip.Scripts;

public partial class PlasmaBolt : Area3D
{
	[Export]
	public float speed = 10f;
	private Vector3 startPos = new Vector3(0, 1.051f, 1.666f);
	public override void _Ready()
	{
		//Set the position to the muzzle of the turret
		this.Position = startPos;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// while the position is less than 1000, move the bolt forward
		if (Position.Z < 30) {
			Position += new Vector3(0, 0, speed * (float) delta);
		} else {
			this.Position = startPos;
		}
	}
	
	public void OnPlayerEntered(Node3D node) {
		//if the bullet hits a player, kil
		if (node is Player player) {
			player.Position = Vector3.Up;
		}
	}
}
