using Godot;
using System;
using Tip.Scripts;

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
		GetNode<GameManager>("/root/GameManager").CurrentLevel = 1;
		GetTree().ChangeSceneToFile("res://Scenes/Build/LoadingScene.tscn");
	}

	public void onQuitButtonPressed()
	{
		GetTree().Quit();
	}

	public void onSettingsPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Build/Settings.tscn");
	}

	public void onCreditsPressed() {
		GetTree().ChangeSceneToFile("res://Scenes/Build/Credits.tscn");
	}


}