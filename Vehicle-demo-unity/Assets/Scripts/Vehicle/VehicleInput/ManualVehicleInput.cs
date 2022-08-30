using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualVehicleInput : VehicleInput {
	
	public HUD hud;
	public GameObject brakeClutchUI;
	private Text brakeClutchTx;
	private Image brakeClutchBG;
	private bool brakeActive;
	
	public void Start() {
		//Move HUD to make room for touch buttons
		if (InputManager.inputType == InputType.Touch) {
			this.hud.gameObject.transform.position += new Vector3(0, 70, 0);
			this.brakeClutchUI.transform.position += new Vector3(0, 70, 0);
		}
		
		this.brakeClutchBG = this.brakeClutchUI.GetComponent<Image>();
		this.brakeClutchTx = this.brakeClutchUI.transform.GetChild(0).GetComponent<Text>();
		SetBrakeActive(true);
	}
	
	public override void Reset() {
		this.controls.Gear = this.vehicle.Config.Power.NeutralIndex;
		SetBrakeActive(true);
	}
	
	public override void UpdateControls() {
		if (InputManager.input.GetAxisAction("ToggleBrakeClutch") > 0)
			SetBrakeActive(! this.brakeActive);
		
		this.controls.Throttle = InputManager.input.GetAxisAction("Throttle");
		this.controls.Brake = this.brakeActive ? InputManager.input.GetAxisAction("Brake") : 0;
		this.controls.SteeringWheel = InputManager.input.GetAxisAction("SteeringWheel");
		this.controls.Clutch = ! this.brakeActive ? InputManager.input.GetAxisAction("Clutch") : 0;
		
		if (InputManager.input.GetButtonAction("GearUp"))
			ChangeGear(1);
		
		if (InputManager.input.GetButtonAction("GearDown"))
			ChangeGear(-1);
	}
	
	private void ChangeGear(int gearDelta) {
		int newGear = this.controls.Gear + gearDelta;
		if (newGear >= 0 && newGear < this.vehicle.Config.Power.GearsCount)
			this.controls.Gear = newGear;
	}
	
	private void SetBrakeActive(bool active) {
		this.brakeActive = active;
		
		if (this.brakeClutchTx != null) {
			this.brakeClutchTx.text = this.brakeActive ? "B" : "C";
			this.brakeClutchBG.color = this.brakeActive ?
				this.hud.Transparent(this.hud.brakeColor) :
				this.hud.Transparent(this.hud.clutchColor);
		}
	}
}