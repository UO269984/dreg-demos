using System;
using System.Collections.Generic;
using UnityEngine;

public class InputSelector : IInput {
	private IDictionary<String, IInput> inputs = new Dictionary<String, IInput>();
	private IInput defaultInput;
	
	public InputSelector(IInput defaultInput) {
		this.defaultInput = defaultInput;
	}
	
	private IInput GetInput(String actionName) {
		IInput input;
		return this.inputs.TryGetValue(actionName, out input) ? input : this.defaultInput;
	}
	
	public bool GetButtonAction(String actionName) {
		return GetInput(actionName).GetButtonAction(actionName);
	}
	
	public float GetAxisAction(String actionName) {
		return GetInput(actionName).GetAxisAction(actionName);
	}
	
	public void AddInput(String actionName, IInput input) {
		this.inputs[actionName] = input;
	}
}