using Godot;
using System;
using Tip.Scripts;

public partial class Button : Area3D {
	[Signal] public delegate void ButtonActivationEventHandler(bool active);

	private int _bodiesPresent;

	public override void _Ready() {
		base._Ready();
		_bodiesPresent = 0;
		BodyEntered += ApplyButtonPusher;
		BodyExited += RemoveButtonPusher;
	}

	private void ApplyButtonPusher(Node3D body) {
		if (body is Box || body is Player) {
			_bodiesPresent++;
		}

		if (_bodiesPresent > 0) {
			EmitSignal(SignalName.ButtonActivation, true);
		}
	}

	private void RemoveButtonPusher(Node3D body) {
		if (body is Box || body is Player) {
			_bodiesPresent--;
		}

		if (_bodiesPresent == 0) {
			EmitSignal(SignalName.ButtonActivation, false);
		}
	}
}
	

