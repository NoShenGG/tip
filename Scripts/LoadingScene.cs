using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
public partial class LoadingScene : Control
{
	//Set as path to intended target scene
	[Export]  String targetScenePath = "res://Scenes/DebugScenes/EthansPusherScene.tscn";

    public override void _Ready()
    {
		ResourceLoader.LoadThreadedRequest(targetScenePath);
        TimedSceneSwap();
    }

	private async void TimedSceneSwap() {
		await Task.Delay(TimeSpan.FromMilliseconds(1500));
		GetTree().ChangeSceneToFile(targetScenePath);
	}
}
