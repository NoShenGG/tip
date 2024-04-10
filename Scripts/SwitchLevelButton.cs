using Godot;
using System;

namespace Tip.Scripts;
public partial class SwitchLevelButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	//When the player presses the button, switch to next level
	public void OnSwitchLevelButtonActivated(Node3D node)
	{
		if (node is Player player)
		{
			GD.Print("Player entered the switch level button");
			// Switch to next Level
			LoadingScene.currLevel++;
			GetTree().ChangeSceneToFile("res://Scenes/DebugScenes/LoadingScene.tscn");
		}
	}
}
