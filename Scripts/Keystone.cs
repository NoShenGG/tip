using System.Reflection.Metadata.Ecma335;
using Godot;



public partial class Keystone : Box
{
	public Vector3 _keyPos = new Vector3(-0.407f, 2.554f, -4.713f);
	public override void _Ready() {
		ProcessMode = Node.ProcessModeEnum.Pausable;
		base._Ready();
		Position = _keyPos;
		GravityScale = 0;
	}
}
