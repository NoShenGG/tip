using Godot;

namespace Tip.Scripts;

public partial class Keystone : Box
{
	private Vector3 _keyPos = new Vector3(-0.407f, 2.554f, -4.713f);
	public override void _Ready() {
		ProcessMode = ProcessModeEnum.Pausable;
		base._Ready();
		Position = _keyPos;
		GravityScale = 0;
	}
}
