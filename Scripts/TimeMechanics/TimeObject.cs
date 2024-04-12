using Godot;

namespace Tip.Scripts.TimeMechanics;

public abstract partial class TimeObject : RigidBody3D, TimeSubscriber {
    protected ObjectHistory Keyframes;
    protected TimeState _currentTimeState;
    private PositionKeyframe _reversalKeyframe;
    private Timer _rewindTimer;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        Keyframes = new ObjectHistory();
        _reversalKeyframe = null;
        FreezeMode = FreezeModeEnum.Kinematic;
        
        // This is NECESSARY to control time behavior. DO NOT TOUCH
        GetNode<TimeManager>("/root/TimeManager").AddSubscriber(this);
    }

    // TODO update this to not rely on materials since we're starting to import new 3d assets
    /*
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
    */

    public override void _PhysicsProcess(double delta) {
        if (_currentTimeState == TimeState.Rewinding) {
            _reversalKeyframe = Keyframes.RemovePositionKeyframe();
            if (_reversalKeyframe != null) {
                Position = _reversalKeyframe.Position;
                Rotation = _reversalKeyframe.Rotation;
            }
        } else if (_currentTimeState == TimeState.Normal) {
            Keyframes.AddPositionKeyframe(Position, Rotation, delta);
        }
    }

    public override void _Notification(int what) {
        base._Notification(what);
        // NECESSARY TO NOT HAVE A MEMORY LEAK AND BAD REFERENCES
        if (what == NotificationPredelete) {
            // GetNode<TimeManager>("/root/TimeManager").RemoveSubscriber(this);
            GD.Print("Removed upon deletion");
        }
    }

    // Necessary to control time behavior, don't modify unless you know what you're doing
    public void UpdateTimeBehavior(TimeState currentState) {
        _currentTimeState = currentState;
        switch (currentState) {
            case TimeState.Normal:
                FreezeMode = FreezeModeEnum.Kinematic;
                Freeze = false;
                break;
            case TimeState.Stopped:
                FreezeMode = FreezeModeEnum.Kinematic;
                Freeze = true;
                break;
            case TimeState.Rewinding:
                FreezeMode = FreezeModeEnum.Kinematic;
                Freeze = true;
                break;
            default:
                GD.PrintErr("Unhandled TimeState!");
                break;
        }
    }
}