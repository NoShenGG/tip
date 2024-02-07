using Godot;

namespace Tip.Scripts.TimeMechanics; 

public class PositionKeyframe {
    public Vector3 Position;
    public Vector3 Rotation;
    public double Delta;

    public PositionKeyframe(Vector3 position, Vector3 rotation, double delta) {
        this.Position = position;
        this.Rotation = rotation;
        this.Delta = delta;
    }
}