using Godot;

using Tip.Scripts.TimeMechanics;

public partial class Player : CharacterBody3D {
	[ExportCategory("Movement")]
	private const float MaxVelocityAir = 0.6f;
	private const float MaxVelocityGround = 6.0f;
	private const float MaxAcceleration = 10 * MaxVelocityGround;
	private const float Gravity = 15.34f;
	private const float StopSpeed = 1.5f;
	private static readonly float JumpImpulse = Mathf.Sqrt(2 * Gravity * 0.85f);
	[Export] private float _friction = 4f;
	
	[ExportCategory("Input")]
	[Export] private float _sensitivity = 0.5f;
	private Node3D _head;
	private Vector3 _movementDir;
	private bool _isJump;
	private bool _checkRaycast;
	private bool _checkPickup;
	private Box _heldItem;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// Locks player mouse to center of screen and hides it for first person controls
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_head = GetNode<Node3D>("Head");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		
	}
	
	// Called every physics update
	public override void _PhysicsProcess(double delta) {
		ProcessMovementInput();
		ProcessMovement(delta);

		if (_checkRaycast) {
			RayCast3D raycast = GetNode<RayCast3D>("Head/Camera3D/RayCast3D");
			if (raycast.GetCollider() is TimeObject timeObject) {
				timeObject.FlipTime();
			}
		}

		// TODO This is hacky and jank and should be abstracted better at some point
		if (_checkPickup) {
			if (_heldItem == null) {
				RayCast3D raycast = GetNode<RayCast3D>("Head/Camera3D/PickupRaycast");
				if (raycast.GetCollider() is Box box) {
					_heldItem = box;
					box.SetPickupPos(GetNode<Node3D>("Head/Camera3D/PickupPos"));
				}
			} else {
				_heldItem.SetPickupPos(null);
				_heldItem = null;
			}
		}
		
		_checkPickup = false;
		_checkRaycast = false;
	}

	private void ProcessMovementInput() {
		_movementDir = Vector3.Zero;

		if (Input.IsActionPressed("forward")) {
			_movementDir -= Transform.Basis.Z;
		} else if (Input.IsActionPressed("backward")) {
			_movementDir += Transform.Basis.Z;
		}
		if (Input.IsActionPressed("left")) {
			_movementDir -= Transform.Basis.X;
		} else if (Input.IsActionPressed("right")) {
			_movementDir += Transform.Basis.X;
		}
		
		// Handle jumping
		_isJump = Input.IsActionJustPressed("jump");
	}

	private void ProcessMovement(double delta) {
		Vector3 normalMoveDir = _movementDir.Normalized();

		if (IsOnFloor()) {
			if (_isJump) {
				Velocity = new Vector3(Velocity.X, JumpImpulse, Velocity.Z);
				Velocity = UpdateVelocityAir(normalMoveDir, delta);
				_isJump = false;
			} else {
				Velocity = UpdateVelocityGround(normalMoveDir, delta);
			}
		} else {
			Velocity = new Vector3(Velocity.X, Velocity.Y - (float) (Gravity * delta), Velocity.Z);
			Velocity = UpdateVelocityAir(normalMoveDir, delta);
		}

		MoveAndSlide();
	}

	private Vector3 Accelerate(Vector3 direction, float maxVelocity, double delta) {
		float currentSpeed = Velocity.Dot(direction);
		float addSpeed = Mathf.Clamp(maxVelocity - currentSpeed, 0, MaxAcceleration * (float) delta);
		return Velocity + (addSpeed * direction);
	}

	private Vector3 UpdateVelocityGround(Vector3 direction, double delta) {
		float speed = Velocity.Length();

		if (speed != 0f) {
			float control = Mathf.Max(StopSpeed, speed);
			float drop = control * _friction * (float) delta;

			Velocity *= Mathf.Max(speed - drop, 0) / speed;
		}

		return Accelerate(direction, MaxVelocityGround, delta);
	}

	private Vector3 UpdateVelocityAir(Vector3 direction, double delta) {
		return Accelerate(direction, MaxVelocityAir, delta);
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);
		
		// Handle mouse input
		if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode.Equals(Input.MouseModeEnum.Captured)) {
			HandleCameraRotation(mouseMotion);
		} else if (@event is InputEventMouseButton mouseButton &&
		           Input.MouseMode.Equals(Input.MouseModeEnum.Captured)) {
			if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left) {
				_checkRaycast = true;
			}
		} else if (@event is InputEventKey key && Input.MouseMode.Equals(Input.MouseModeEnum.Captured)) {
			if (key.PhysicalKeycode == Key.E && key.Echo == false && key.Pressed == true) {
				_checkPickup = true;
			}
		}
	}

	private void HandleCameraRotation(InputEventMouseMotion mouseMotion) {
		RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * _sensitivity));
		_head.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * _sensitivity));

		Vector3 clampedRotation = new Vector3(
			Mathf.Clamp(_head.Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(90)),
			_head.Rotation.Y,
			_head.Rotation.Z);
		_head.Rotation = clampedRotation;
	}
}