using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tip.Scripts.TimeMechanics;

public partial class LoadingScene : Control
{
	//Set as path to intended target scene
	private string targetScenePath;
	public static int currLevel = 1;
    public override void _Ready()
    {
		
		targetScenePath = "res://Scenes/Build/Level" + currLevel + ".tscn";
		ResourceLoader.LoadThreadedRequest(targetScenePath);
        TimedSceneSwap();
    }

	private async void TimedSceneSwap() {
		await Task.Delay(TimeSpan.FromMilliseconds(1500));
		GetNode<TimeManager>("/root/TimeManager").Reset();
		GetTree().ChangeSceneToFile(targetScenePath);
	}
}
