using Godot;
using System;

public partial class LaserBlaster : RigidBody3D
{
	public PackedScene BoltScene { get; set; }
	
    public override void _Ready()
    {
		//Make the turret unmovable by forces
		this.Freeze = true;
		//load the laser scene
		BoltScene = GD.Load<PackedScene>("res://Scenes/Models/Laser/PlasmaBolt.tscn");
		//create an instance of the laser bolt
		var instance = BoltScene.Instantiate();
		//Add it as a child of the blaster (I think this is good for keeping direction consistent? Maybe? not sure.)
		AddChild(instance);
    }
}
