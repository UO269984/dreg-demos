using System;
using UnityEngine;

public class DefaultInput : IInput {
	
	public bool GetButtonAction(String actionName) {
		return Input.GetButton(actionName);
	}
	
	public float GetAxisAction(String actionName) {
		return Input.GetAxis(actionName);
	}
}