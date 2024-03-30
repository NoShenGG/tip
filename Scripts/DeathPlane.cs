using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace Tip.Scripts;

public partial class DeathPlane : Area3D
{
	public Vector3 spawnPoint;
	[Export]
	public Player player;
	//Drag and drop the elements node where the boxes and stuff SHOULD BE PLACED!!!!!!!!!111!!11!11!!111!
	[Export]
	private Node3D elementsToRespawn;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		spawnPoint = player.Transform.Origin;
		BodyEntered += KillEntity;	
	}

	private void KillEntity(Node3D entity) {
		if (entity is Box box) {
			box.Respawn();
		} else if (entity is Player player) {
			player.Position = spawnPoint;
			//Respawns all the boxes under the element node to prevent softlock (please stay organized)
			foreach (Node3D i in elementsToRespawn.GetChildren()) {
				if (i is Box) {
					((Box) i).Respawn();
				}
			}
		}
	}
}