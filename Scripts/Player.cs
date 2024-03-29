using Godot;
using Tip.Scripts.TimeMechanics;

namespace Tip.Scripts;

public partial class Player : CharacterBody3D, TimeSubscriber {
	
	#region Movement Variables
	
	private const float MaxVelocityAir = 0.6f;
	private const float MaxVelocityGround = 6.0f;
	private const float MaxAcceleration = 10 * MaxVelocityGround;
	private const float Gravity = 15.34f;
	private const float StopSpeed = 1.5f;
	private static readonly float JumpImpulse = Mathf.Sqrt(2 * Gravity * 0.85f);
	[ExportCategory("Movement")]
	[Export] private float _friction = 4f;
	
	#endregion
	
	#region Input Variables
	
	[ExportCategory("Input")]
	[Export] private float _sensitivity = 0.5f;
	private Node3D _head;
	private Node3D _pickupPos;
	private Vector3 _movementDir;
	private bool _isJump;
	
	#endregion
	
	#region Pickup Variables
	
	[ExportCategory("Pickup")]
	[Export] private float _pickUpTrackModifier = 20.0f;
	[Export] private float _maxPickupVelocity = 10.0f;
	private TimeState _currentTimeState;
	private bool _checkPickup;
	private Box _heldItem;
	
	#endregion
	
	#region Godot Override
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// Locks player mouse to center of screen and hides it for first person controls
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_head = GetNode<Node3D>("Head");
		_pickupPos = GetNode<Node3D>("Head/Camera3D/PickupPos");
		
		// This is NECESSARY to control time behavior. DO NOT TOUCH
		GetNode<TimeManager>("/root/TimeManager").AddSubscriber(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		
	}
	
	// Called every physics update
	public override void _PhysicsProcess(double delta) {
		ProcessMovementInput();
		ProcessMovement(delta);
		
		// Grab instance for pick up
		if (_checkPickup) {
			if (!IsInstanceValid(_heldItem)) {
				RayCast3D raycast = GetNode<RayCast3D>("Head/Camera3D/PickupRaycast");
				if (raycast.GetCollider() is Box box) {
					// TODO Change this to not be like this omg this sucks actually
					if (box is not Platform) {
						_heldItem = box;
						_heldItem.AngularVelocity = Vector3.Zero;
					}
				}
			} else {
				_heldItem.EnableTimeBehavior = true;
				if (_currentTimeState != TimeState.Normal) {
					_heldItem.Freeze = true;
				}

				_heldItem.AngularVelocity = Vector3.Zero;
				_heldItem = null;
			}
		}
		
		// Handle pick up physics
		if (IsInstanceValid(_heldItem)) {
			switch (_currentTimeState) {
				case TimeState.Normal:
					_heldItem.EnableTimeBehavior = true;
					break;
				case TimeState.Rewinding:
					_heldItem.EnableTimeBehavior = true;
					_heldItem = null;
					break;
				case TimeState.Stopped:
					_heldItem.EnableTimeBehavior = false;
					_heldItem.Freeze = false;
					break;
				default:
					GD.PrintErr("Unhandled TimeState!");
					break;
			}

			// Double-check held item valid after time changes
			if (IsInstanceValid(_heldItem)) {
				Vector3 dir = _heldItem.GlobalPosition.DirectionTo(_pickupPos.GlobalPosition);
				float mag = _heldItem.GlobalPosition.DistanceTo(_pickupPos.GlobalPosition);
				
				if (mag > _maxPickupVelocity) {
					_heldItem = null;
				} else {
					_heldItem.LinearVelocity = dir * mag * _pickUpTrackModifier;
					_heldItem.Rotation = _pickupPos.GlobalRotation;
					_heldItem.Rotation = new Vector3(0, _heldItem.Rotation.Y, _heldItem.Rotation.Z);
				}
			}
		}
		
		_checkPickup = false;
	}
	
	public override void _Input(InputEvent @event) {
		base._Input(@event);
		
		// Handle mouse input
		if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode.Equals(Input.MouseModeEnum.Captured)) {
			HandleCameraRotation(mouseMotion);
		} else if (@event.IsActionPressed("interact") && Input.MouseMode.Equals(Input.MouseModeEnum.Captured)) {
			_checkPickup = true;
		}
	}
	
	#endregion

	#region Movement
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
	
	#endregion

	#region Camera
	private void HandleCameraRotation(InputEventMouseMotion mouseMotion) {
		RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * _sensitivity));
		_head.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * _sensitivity));

		Vector3 clampedRotation = new Vector3(
			Mathf.Clamp(_head.Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(90)),
			_head.Rotation.Y,
			_head.Rotation.Z);
		_head.Rotation = clampedRotation;
	}
	#endregion

	#region Time-related Methods

	// Necessary to control time behavior, don't modify unless you know what you're doing
	public void UpdateTimeBehavior(TimeState currentState) {
		_currentTimeState = currentState;
	}

	#endregion
}