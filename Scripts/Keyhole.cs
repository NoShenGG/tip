using Godot;

namespace Tip.Scripts;

public partial class Keyhole : Area3D
{
    public void OnKeyholeBodyEntered(Node3D body)
    {
        if (body is Keystone keystone)
        {
            GD.Print("Keystone was inserted into keyhole!");
        }
    }
}
