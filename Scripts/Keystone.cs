using Godot;
using Tip.Scripts.TimeMechanics;

namespace Tip.Scripts;

public partial class Keystone : Box
{
	public override void _Ready() {
		base._Ready();
		Position = new Vector3(-0.8f, 1f, -0.25f);
	}

	public void OnForcegrabberEntered(Node3D body) 
	{
		if (body is Keystone keystone)
        {
            GD.Print("Keystone was inserted into force grabber!");
			Player player = GetNode<Player>("../Player");
			this.Position = Vector3.Zero;

        }
		
	}
}
