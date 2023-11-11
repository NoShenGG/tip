using Godot;
using System;
using System.Data;

public partial class Player : CharacterBody3D {
	[ExportCategory("Input")]
	[Export]
	private float sensitivity = 1.272f;
	private Node3D _head;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_head = GetNode<Node3D>("Head");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (@event is InputEventMouseMotion && Input.MouseMode.Equals(Input.MouseModeEnum.Captured)) {
			HandleCameraRotation(@event);
		}
	}

	public void HandleCameraRotation(InputEvent @event) {
		InputEventMouseMotion mouseMotion = (InputEventMouseMotion) @event;
		RotateY(Mathf.DegToRad(-mouseMotion.Relative.Y * sensitivity));
		_head.RotateX(Mathf.DegToRad(-mouseMotion.Relative.X * sensitivity));

		Vector3 clampedRotation = new Vector3(
			Mathf.Clamp(_head.Rotation.X, Mathf.DegToRad(-60), Mathf.DegToRad(90)),
			_head.Rotation.Y,
			_head.Rotation.Z);
		_head.Rotation = clampedRotation;
	}
}