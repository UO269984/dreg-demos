using System;

public interface IInput {
	public bool GetButtonAction(String actionName);
	public float GetAxisAction(String actionName);
}