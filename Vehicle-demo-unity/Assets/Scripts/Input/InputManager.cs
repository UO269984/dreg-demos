using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType {
	Gamepad, Touch, Default
}

public class InputManager : MonoBehaviour {
	
	[System.Serializable]
	public struct InputAction {
		public String name;
		public GamepadBt gamepadBt;
		public TouchButton uiBt;
		public bool trigger;
	}
	
	public InputAction[] actions;
	public String gyroscopeAction;
	public String[] defaultActionsInTouch;
	
	public static IInput input;
	public static InputType inputType = InputType.Default;
	
	public void Start() {
		CreateInput();
		
		TriggerInput triggerInput = new TriggerInput(input);
		foreach (InputAction action in this.actions) {
			if (action.trigger)
				triggerInput.AddTriggerAction(action.name);
		}
		input = triggerInput;
	}
	
	private void CreateInput() {
		if (GamepadInput.IsActive()) {
			GamepadInput gamepadInput = new GamepadInput();
			input = gamepadInput;
			inputType = InputType.Gamepad;
			
			foreach (InputAction action in this.actions)
				gamepadInput.AddAction(action.name, action.gamepadBt);
		}
		
		else if (TouchInput.IsActive() && GyroscopeInput.IsActive()) {
			TouchInput touchInput = new TouchInput();
			
			foreach (InputAction action in this.actions) {
				if (action.uiBt != null)
					touchInput.AddAction(action.name, action.uiBt);
			}
			DefaultInput defaultInput = new DefaultInput();
			InputSelector inputSelector = new InputSelector(touchInput);
			inputSelector.AddInput(this.gyroscopeAction, new GyroscopeInput(25));
			
			foreach (String actionName in this.defaultActionsInTouch)
				inputSelector.AddInput(actionName, defaultInput);
			
			input = inputSelector;
			inputType = InputType.Touch;
		}
		else
			input = new DefaultInput();
	}
}