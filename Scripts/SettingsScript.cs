using Godot;
using System;

public partial class SettingsScript : Control
{
	public void onBackButtonPressed() {
		GetTree().ChangeSceneToFile("res://Scenes/Build/StartScene.tscn");
	}
	
}
