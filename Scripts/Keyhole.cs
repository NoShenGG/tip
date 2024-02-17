using Godot;
using Tip.Scripts.TimeMechanics;

namespace Tip.Scripts;

public partial class Keyhole : Area3D
{
    public Vector3 globalPosition {get; set;}
    public Vector3 globalRotation {get; set;}
    public override void _Ready()
    {
        base._Ready();
        globalPosition = this.GlobalPosition;
        globalRotation = this.GlobalRotation;
    }
    public void OnKeyholeBodyEntered(Node3D body)
    {
        if (body is Keystone keystone)
        {
            GD.Print("Keystone was inserted into keyhole!");
        }
    }
}
