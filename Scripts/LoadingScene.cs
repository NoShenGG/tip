using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tip.Scripts.TimeMechanics;

public partial class LoadingScene : Control
{
	//Set as path to intended target scene
	[Export]  String targetScenePath = "res://Scenes/Build/Level1.tscn";

    public override void _Ready()
    {
		ResourceLoader.LoadThreadedRequest(targetScenePath);
        TimedSceneSwap();
    }

	private async void TimedSceneSwap() {
		await Task.Delay(TimeSpan.FromMilliseconds(1500));
		GetNode<TimeManager>("/root/TimeManager").Reset();
		GetTree().ChangeSceneToFile(targetScenePath);
	}
}
