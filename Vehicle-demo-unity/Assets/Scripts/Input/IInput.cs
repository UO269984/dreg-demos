using System;

public interface IInput {
	public bool IsActive();
	
	public bool GetButtonAction(String actionName);
	public float GetAxisAction(String actionName);
}