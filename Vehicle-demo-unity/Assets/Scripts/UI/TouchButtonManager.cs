using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButtonManager : MonoBehaviour {
	
	public VehicleCamera vehicleCamera;
	private int touchButtonsPressed = 0;
	
	public void LoadButtons() {
		foreach (TouchButton bt in GameObject.FindObjectsOfType(typeof(TouchButton))) {
			bt.MouseDownCallback = this.ButtonDown;
			bt.MouseUpCallback = this.ButtonUp;
		}
	}
	
	public void ButtonDown() {
		if (this.touchButtonsPressed == 0)
			this.vehicleCamera.enabled = false;
		
		this.touchButtonsPressed++;
	}
	
	public void ButtonUp() {
		this.touchButtonsPressed--;
		if (this.touchButtonsPressed == 0)
			this.vehicleCamera.enabled = true;
	}
}