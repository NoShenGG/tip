using Godot;

namespace Tip.Scripts.TimeMechanics;

public abstract partial class TimeObject : RigidBody3D {
    protected ObjectHistory _objectHistory;
    private bool _isReversing;
    private PositionKeyframe _reversalKeyframe;
    private Timer _rewindTimer;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        _objectHistory = new ObjectHistory();
        _isReversing = false;
        _reversalKeyframe = null;
        
        // This is NECESSARY to control time behavior. DO NOT TOUCH
        GetNode<TimeManager>("/root/TimeManager").AddSubscriber(this);
    }

    public override void _Process(double delta) {
        if (_isReversing) {
            StandardMaterial3D rewindMaterial = new StandardMaterial3D();
            rewindMaterial.AlbedoColor = Colors.Yellow;
            GetNode<MeshInstance3D>("MeshInstance3D").MaterialOverride = rewindMaterial;
        } else {
            StandardMaterial3D normalMaterial = new StandardMaterial3D();
            normalMaterial.AlbedoColor = Colors.Black;
            GetNode<MeshInstance3D>("MeshInstance3D").MaterialOverride = normalMaterial;
        }
    }

    public override void _PhysicsProcess(double delta) {
        if (_isReversing) {
            _reversalKeyframe = _objectHistory.RemovePositionKeyframe();
            if (_reversalKeyframe != null) {
                Position = _reversalKeyframe.position;
                Rotation = _reversalKeyframe.rotation;
            }
        } else if (!Freeze) {
            _objectHistory.AddPositionKeyframe(Position, Rotation, delta);
        }
    }

    public override void _Notification(int what) {
        base._Notification(what);
        // NECESSARY TO NOT HAVE A MEMORY LEAK AND BAD REFERENCES
        if (what == NotificationPredelete) {
            GetNode<TimeManager>("/root/TimeManager").RemoveSubscriber(this);
        }
    }

    // Necessary to control time behavior, don't modify unless you know what you're doing
    public void UpdateTimeBehavior(TimeState currentState) {
        switch (currentState) {
            case TimeState.Normal:
                _isReversing = false;
                Freeze = false;
                break;
            case TimeState.Stopped:
                _isReversing = false;
                Freeze = true;
                break;
            case TimeState.Rewinding:
                _isReversing = true;
                Freeze = true;
                break;
            default:
                GD.PrintErr("Unhandled TimeState!");
                break;
        }
    }
}