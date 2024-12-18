using Godot;
using Godot.Collections;

namespace Tip.Scripts.DEBUG; 

public partial class SuperimposePlatform : Node3D {
    [Export] private Button button = new Button();
    [Export] private bool _inverseBehavior = false;
    
    public override void _Ready() {
        Visible = !_inverseBehavior;
        Array disablePropArray = new Array();
        disablePropArray.Add("disabled");
        disablePropArray.Add(_inverseBehavior);
        PropagateCall("set", disablePropArray);
        if (button == null) {
            GD.PrintErr("BUTTON NOT DEFINED!");
        } else {
            button.ButtonActivation += ToggleBehavior;
        }
    }

    private void ToggleBehavior(bool isActive) {
        if (isActive) {
            Visible = _inverseBehavior;
            Array disablePropArray = new Array();
            disablePropArray.Add("disabled");
            disablePropArray.Add(!_inverseBehavior);
            PropagateCall("set", disablePropArray);
        } else {
            Visible = !_inverseBehavior;
            Array disablePropArray = new Array();
            disablePropArray.Add("disabled");
            disablePropArray.Add(_inverseBehavior);
            PropagateCall("set", disablePropArray);
        }
    }
}