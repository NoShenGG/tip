using Godot;

public partial class AntiAliasing : OptionButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Select(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (this.GetSelectedId()) {
			case 0:
				ProjectSettings.SetSetting("rendering/anti_aliasing/quality/screen_space_aa", 0);
				break;
			case 1:
				ProjectSettings.SetSetting("rendering/anti_aliasing/quality/screen_space_aa", 1);
				break;
			case 2:
				ProjectSettings.SetSetting("rendering/anti_aliasing/quality/msaa_3d", 2);
				break;
			case 3:
				ProjectSettings.SetSetting("rendering/anti_aliasing/quality/msaa_3d", 4);
				break;
			default:
				break;
		}
	}
}
