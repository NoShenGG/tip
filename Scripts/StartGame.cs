using Godot;
using System;

public partial class StartGame : Control
{
	private VBoxContainer menu = null;

	public override void _Ready()
	{
		menu = GetNode<VBoxContainer>("Menu");

	}

	// Called when the node enters the scene tree for the first time.
	public void onStartButtonPressed()
	{
		LoadingScene.currLevel = 1;
		GetTree().ChangeSceneToFile("res://Scenes/DebugScenes/LoadingScene.tscn");
	}

	public void onQuitButtonPressed()
	{
		GetTree().Quit();
	}

	public void onSettingsPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/DebugScenes/Settings.tscn");
	}


}
