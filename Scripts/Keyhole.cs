using Godot;

public partial class Keyhole : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnKeyholeBodyEntered;
    }
    
    public void OnKeyholeBodyEntered(Node3D body)
    {
        if (body is Keystone keystone)
        {
            GD.Print("Keystone was inserted into keyhole!");
        }
    }
}
