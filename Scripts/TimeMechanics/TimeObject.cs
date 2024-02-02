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

    public void FlipTime() {
        if (_isReversing) {
            StopRewind();
        } else {
            StartRewind();
        }
    }

    private void StartRewind() {
        _isReversing = true;
        Freeze = true;
        _rewindTimer = new Timer();
        _rewindTimer.WaitTime = 10.0;
        _rewindTimer.Timeout += StopRewind;
        _rewindTimer.Autostart = true;
    }

    private void StopRewind() {
        _rewindTimer.Timeout -= StopRewind;
        _rewindTimer.Stop();
        _rewindTimer.Free();
        Freeze = false;
        _isReversing = false;
    }
}