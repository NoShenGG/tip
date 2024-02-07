using Godot;
using System;

public partial class RecordingCamera : Camera3D
{
	public override void _Input(InputEvent @event) {
		base._Input(@event);
		if (@event is InputEventKey key) {
			if (key.PhysicalKeycode == Key.KpEnter && !key.Echo && key.Pressed) {
				Current = !Current;
				GetNode<TextureRect>("/root/Recording/Player/Head/Camera3D/TextureRect").Visible =
					!GetNode<TextureRect>("/root/Recording/Player/Head/Camera3D/TextureRect").Visible;
			}
		}
	}
}
