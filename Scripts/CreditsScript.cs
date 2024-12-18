using Godot;
using System;

public partial class CreditsScript : Control
{
    public void onBackButtonPressed() {
        GetTree().ChangeSceneToFile("res://Scenes/Build/StartScene.tscn");
    }
}