using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tip.Scripts;
using Tip.Scripts.TimeMechanics;

public partial class LoadingScene : Control
{
	//Set as path to intended target scene
	private string targetScenePath;
	private int _currLevel;
	
    public override void _Ready()
    {
	    _currLevel = GetNode<GameManager>("/root/GameManager").CurrentLevel;
	    if (_currLevel > 2) {
		    Input.MouseMode = Input.MouseModeEnum.Visible;
		    targetScenePath = "res://Scenes/Build/StartScene.tscn";
	    } else {
		    targetScenePath = "res://Scenes/Build/Level" + _currLevel + ".tscn";
	    }
	    ResourceLoader.LoadThreadedRequest(targetScenePath);
	    TimedSceneSwap();
    }

	private async void TimedSceneSwap() {
		await Task.Delay(TimeSpan.FromMilliseconds(1500));
		GetNode<TimeManager>("/root/TimeManager").Reset();
		GetTree().ChangeSceneToFile(targetScenePath);
	}
}
