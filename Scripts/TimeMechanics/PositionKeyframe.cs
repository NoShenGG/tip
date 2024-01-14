using Godot;

namespace Tip.Scripts.TimeMechanics; 

public class PositionKeyframe {
    public Vector3 position;
    public double delta;

    public PositionKeyframe(Vector3 position, double delta) {
        this.position = position;
        this.delta = delta;
    }
}