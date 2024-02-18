namespace Tip.Scripts.TimeMechanics; 

public interface TimeSubscriber {
	void UpdateTimeBehavior(TimeState currentState);
}