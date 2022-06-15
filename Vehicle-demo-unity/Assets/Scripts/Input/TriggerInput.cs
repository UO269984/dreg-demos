using System;
using System.Collections.Generic;

public class TriggerInput : IInput {
	private IInput input;
	private IDictionary<String, bool> triggeredActions = new Dictionary<String, bool>();
	
	public TriggerInput(IInput input) {
		this.input = input;
	}
	
	public void AddTriggerAction(String actionName) {
		this.triggeredActions[actionName] = false;
	}
	
	public bool IsActive() {
		return this.input.IsActive();
	}
	
	public bool GetButtonAction(String actionName) {
		bool prevTriggered;
		
		if (this.triggeredActions.TryGetValue(actionName, out prevTriggered)) {
			bool nowTriggered = this.input.GetButtonAction(actionName);
			this.triggeredActions[actionName] = nowTriggered;
			
			return nowTriggered && ! prevTriggered;
		}
		else
			return this.input.GetButtonAction(actionName);
	}
	
	public float GetAxisAction(String actionName) {
		bool prevTriggered;
		
		if (this.triggeredActions.TryGetValue(actionName, out prevTriggered)) {
			float axisVal = this.input.GetAxisAction(actionName);
			bool nowTriggered = axisVal != 0;
			this.triggeredActions[actionName] = nowTriggered;
			
			return nowTriggered && ! prevTriggered ? axisVal : 0;
		}
		else
			return this.input.GetAxisAction(actionName);
	}
}