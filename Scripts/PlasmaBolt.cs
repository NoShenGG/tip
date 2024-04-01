using Godot;
using System;
using Tip.Scripts;

public partial class PlasmaBolt : Area3D
{
	[Export]
	public float speed = 20f;
	
	public Vector3 startPos = new Vector3(0, 1.051f, 1.666f);	
	private Vector3 spawnPoint;
	
	public override void _Ready()
	{
		//Reuse spawnpoint from the deathplane class
		spawnPoint = DeathPlane.spawnPoint;
		shootLaser();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// while the position is less than 1000, move the bolt forward
		if (Position.Z < 30) {
			Position += new Vector3(0, 0, speed * (float) delta);
		} else {
			shootLaser();
		}
	}
	
	public void OnPlayerEntered(Node3D node) {
		//if the bullet hits a player, kil
		if (node is Player player) {
			player.Position = spawnPoint;
		}
	}

	private void shootLaser() {
		//Set the position to the muzzle of the turret
		this.Position = startPos;
		//Play shooty noise
		GetNode<AudioStreamPlayer3D>("SoundLaser").Play();
	}
}
