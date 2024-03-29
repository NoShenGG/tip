using Godot;

namespace Tip.Scripts;

public partial class Keyhole : Area3D {
    private bool _completed;
    
    public override void _Ready() {
        _completed = false;
        BodyEntered += EndLevel;
    }

    private void EndLevel(Node3D body)
    {
        if (body is Keystone keystone)
        {
            GD.Print("Keystone was inserted into keyhole!");
        }
        CsgBox3D divider = GetNode<CsgBox3D>("/root/Level1/BoxRoom/SandboxDivider");
        if (IsInstanceValid(divider) && !_completed) {
            divider.Visible = false;
            PopupText();
            _completed = true;
        }
    }

    private async void PopupText() {
        Label sandboxText = GetNode<Label>("/root/Level1/Player/CanvasLayer/SandboxAreaText");
        sandboxText.Visible = true;
        await ToSignal(GetTree().CreateTimer(3.0f), SceneTreeTimer.SignalName.Timeout);
        sandboxText.Visible = false;
    }
}
