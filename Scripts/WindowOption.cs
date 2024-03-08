using Godot;
using System;

public partial class WindowOption : OptionButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Select(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (this.GetSelectedId()) {
			case 0:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				break;
			case 1:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
				break;
			default:
				break;
		}
	}
}
