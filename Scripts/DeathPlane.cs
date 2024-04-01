using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using Tip.Scripts.TimeMechanics;

namespace Tip.Scripts;

public partial class DeathPlane : Area3D
{

	//Declare player and elements to respawn
	private Player player;
	private Node3D elementsToRespawn;
	public static Vector3 spawnPoint {get; private set;}
	//Write the level to find the player and elements to respawn
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		player = GetTree().CurrentScene.GetNode<Player>("/root/Level" + LoadingScene.currLevel + "/Player");
		elementsToRespawn = GetTree().CurrentScene.GetNode<Node3D>("/root/Level" + LoadingScene.currLevel + "/Elements");
		spawnPoint = player.Transform.Origin;

		BodyEntered += KillEntity;	
	}

	private void KillEntity(Node3D entity) {
		if (entity is Box box) {
			box.Respawn();
		} else if (entity is Player player) {
			player.Position = spawnPoint;
			//Set time state back to normal to avoid glitches
			foreach (Node3D i in elementsToRespawn.GetChildren()) {
				if (i is Box) {
					((Box) i).Respawn();
				}
			}
		}
	}
}