using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	
	[System.Serializable]
	public struct InputAction {
		public String name;
		public GamepadBt bt;
	}
	
	public InputAction[] actions;
	
	public static IInput input;
	private GamepadInput gamepadInput;
	
	void Start() {
		this.gamepadInput = new GamepadInput();
		foreach (InputAction action in this.actions)
			this.gamepadInput.AddAction(action.name, action.bt);
		
		input = this.gamepadInput.IsActive() ? (IInput) this.gamepadInput : (IInput) new DefaultInput();
	}
}