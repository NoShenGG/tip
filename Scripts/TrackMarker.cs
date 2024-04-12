using Godot;
using Tip.Scripts.TimeMechanics;

namespace Tip.Scripts;

public partial class TrackMarker : TimeObject {
    public int RewindCount;
    private const float MaxPositionDeviance = 0.1f;
    public override void _Ready() {
        base._Ready();
        GravityScale = 0;
        RewindCount = 0;
    }

    public override void _PhysicsProcess(double delta) {
        Vector3 oldPosition = Position;
        base._PhysicsProcess(delta);
        if (_currentTimeState == TimeState.Rewinding && oldPosition.DistanceTo(Position) > MaxPositionDeviance) {
            RewindCount++;
        }
    }
}
