using System;
using Godot;
using Godot.Collections;
using Tip.Scripts.TimeMechanics;

namespace Tip.Scripts;

public partial class Platform : Box {
    [Export] private bool _onRails = false;
    [Export] private Array<Vector3> _trackPoints = new Array<Vector3>();
    [Export] private float _trackSpeed = 1.0f;
    [Export] private float _stallTime = 0.3f;
    
    private TrackMarker _trackMarker;
    private int _currentTrackPoint;
    private float _stallTimeRemaining;

    public override void _Ready() {
        base._Ready();
        if (_onRails) {
            _trackMarker = new TrackMarker();
            GetParent().CallDeferred(Node.MethodName.AddChild, _trackMarker);
            _trackMarker.Position = _trackPoints[0];
            _currentTrackPoint = 0;
            _stallTimeRemaining = 0.0f;
            GravityScale = 0;
        }
    }

    public override void _PhysicsProcess(double delta) {
        if (_onRails && _currentTimeState == TimeState.Normal) {
            Freeze = true;
            if (_trackMarker.RewindCount > 0) {
                _currentTrackPoint -= _trackMarker.RewindCount;
                _trackMarker.RewindCount = 0;
            }
            if (_stallTimeRemaining >= 0) {
                _stallTimeRemaining -= (float) delta;
            } else {
                if (Position.DistanceTo(_trackMarker.Position) > (_trackSpeed / 10)) {
                    Position += Position.DirectionTo(_trackMarker.Position) * (_trackSpeed / 10);
                } else {
                    Position = _trackMarker.Position;
                    _stallTimeRemaining = _stallTime;
                    GoToNextTrackPoint();
                }
            }
        }
        base._PhysicsProcess(delta);
    }

    private void GoToNextTrackPoint() {
        _currentTrackPoint++;
        if (_currentTrackPoint >= _trackPoints.Count) {
            _currentTrackPoint = 0;
            Respawn();
        }
        
        _trackMarker.Position = _trackPoints[_currentTrackPoint];
    }
}
