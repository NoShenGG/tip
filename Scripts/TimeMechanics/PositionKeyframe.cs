using Godot;

namespace Tip.Scripts.TimeMechanics; 

public class PositionKeyframe {
    public Vector3 position;
    public Vector3 rotation;
    public double delta;

    public PositionKeyframe(Vector3 position, Vector3 rotation, double delta) {
        this.position = position;
        this.rotation = rotation;
        this.delta = delta;
    }
}