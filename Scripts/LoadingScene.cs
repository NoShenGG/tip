using Godot;
using System;
using System.Text.RegularExpressions;

public partial class LoadingScene : Control
{
	//Set as path to intended target scene
	private const String targetScenePath = "res://Scenes/DebugScenes/ExampleWorld.tscn";
	private ProgressBar progressBar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		progressBar = GetNode<ProgressBar>("ProgressBar");
		GetTree().ChangeSceneToFile("res://Scenes/DebugScenes/ExampleWorld.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//TODO: make it work
		//Lol I have no idea how to code a progress bar so I'm just gonna make it go to 100 :)))
		progressBar.Value = 100;
	}

}
