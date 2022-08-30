using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : IInput {
	private IDictionary<String, TouchButton> actions = new Dictionary<String, TouchButton>();
	
	public static bool IsActive() {
		return Application.platform == RuntimePlatform.Android;
	}
	
	public bool GetButtonAction(String actionName) {
		TouchButton bt;
		return this.actions.TryGetValue(actionName, out bt) ? bt.Pressed : false;
	}
	
	public float GetAxisAction(String actionName) {
		return GetButtonAction(actionName) ? 1 : 0;
	}
	
	public void AddAction(String actionName, TouchButton uiBt) {
		this.actions[actionName] = uiBt;
	}
}