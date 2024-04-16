using Godot;
using System;
using Tip.Scripts;

public partial class PlasmaBolt : Area3D
{
	[Export]
	public float speed = 20f;
	[Export] private Vector3 _playerSpawnPos = Vector3.Zero;
	[Export] private Vector3 _playerSpawnRot = Vector3.Zero;
	public Vector3 startPos = new Vector3(0, 1.051f, 1.666f);	
	
	public override void _Ready()
	{
		//Reuse spawnpoint from the deathplane class
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
			player.Position = _playerSpawnPos;
			player.Rotation = _playerSpawnRot;
			player.Velocity = Vector3.Zero;
		}
	}

	private void shootLaser() {
		//Set the position to the muzzle of the turret
		this.Position = startPos;
		//Play shooty noise
		GetNode<AudioStreamPlayer3D>("SoundLaser").Play();
	}
}
