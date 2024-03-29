using System.Collections.Generic;
using Godot;

namespace Tip.Scripts.TimeMechanics; 

public partial class TimeManager : Node {
    // Used to emulate Singleton behavior via Godot's Autoload system
    // DO NOT ATTACH MANUALLY TO ANY NODE
    // DO NOT EDIT UNLESS YOU KNOW WHAT YOU ARE DOING
    // THIS SHOULD BE ACCESSIBLE VIA THE METHODS DESCRIBED HERE
    // https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html

    private List<TimeSubscriber> _subscribers;
    private TimeState _currentTimeState;

    public override void _Ready() {
        base._Ready();
        _subscribers = new List<TimeSubscriber>();
        _currentTimeState = TimeState.Normal;
        GD.Print("Loaded!");
    }
    
    /// <summary>
    /// <para>Used to add TimeObjects to the TimeManager whenever they are instantiated.</para>
    /// <para><b>Should be called by default in all TimeObjects.</b></para>
    /// </summary>
    /// <param name="newSubscriber">the new TimeObject to add to the list of TimeManager subscribers</param>
    public void AddSubscriber(TimeSubscriber newSubscriber) {
        _subscribers.Add(newSubscriber);
    }
    
    /// <summary>
    /// <para>Used to remove TimeObjects from the TimeManager whenever they are freed, removed, etc.</para>
    /// <para><b>Should be called by default in all TimeObjects when they are freed.</b></para>
    /// </summary>
    /// <param name="currentSubscriber">the TimeObject to remove from the list of TimeObjects</param>
    /// <returns>True: if subscriber exists in subscriber list<br/>
    /// False: if subscriber does not exist in subscriber list</returns>
    public bool RemoveSubscriber(TimeSubscriber currentSubscriber) {
        if (_subscribers.Contains(currentSubscriber)) {
            _subscribers.Remove(currentSubscriber);
            return true;
        }
        return false;
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (@event.IsActionPressed("primary_fire") && _currentTimeState != TimeState.Stopped) {
            _currentTimeState = TimeState.Stopped;
            NotifySubscribers();
        } else if (@event.IsActionPressed("secondary_fire") && _currentTimeState != TimeState.Rewinding) {
            _currentTimeState = TimeState.Rewinding;
            NotifySubscribers();
        } else if (@event.IsActionPressed("cancel") && _currentTimeState != TimeState.Normal) {
            _currentTimeState = TimeState.Normal;
            NotifySubscribers();
        }
    }

    private void NotifySubscribers() {
        foreach (TimeSubscriber t in _subscribers) {
            t.UpdateTimeBehavior(_currentTimeState);
        }
    }

    public override void _UnhandledInput(InputEvent @event) {
        if (@event.IsActionPressed("DEBUG_RESET")) {
            Reset();
            GetViewport().SetInputAsHandled();
            Input.MouseMode = Input.MouseModeEnum.Visible;
            GetTree().ChangeSceneToFile("res://Scenes/DebugScenes/StartScene.tscn");
        }
    }

    public void Reset() {
        _subscribers.Clear();
        _currentTimeState = TimeState.Normal;
    }
}