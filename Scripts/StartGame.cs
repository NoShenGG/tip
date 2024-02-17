using Godot;
using System;

public partial class StartGame : Control
{

	// Called when the node enters the scene tree for the first time.
	public void onButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/DebugScenes/LoadingScene.tscn");
	}

}
