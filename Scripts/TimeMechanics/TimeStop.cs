using Godot;
using System;

namespace Tip.Scripts.TimeMechanics; 

public partial class TimeStop : Node
{
	[Export] 
    private bool canTogglePause = true;
	
	public void _process(double delta) {
		if (Input.IsActionJustPressed("time_stop")) {
			if (!GetTree().Paused) {
				stopTime();
			} else {
				resumeTime();
			} 
		}
	}
	public void stopTime() {
		if (canTogglePause) {
			GetTree().SetDeferred("paused", true);
		}
	}

	public void resumeTime() {
		if (canTogglePause) {
			GetTree().SetDeferred("paused", false);
		}
	}
}
