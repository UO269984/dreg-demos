using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticVehicleInput : VehicleInput {
	
	public float clutchPressTime = 0;
	public float releaseClutchRpm;
	public float pressClutchRpm;
	
	protected bool changingGear;
	private int gearChangeDelta;
	private bool pressingClutch;
	
	public override void Reset() {
		this.controls.Clutch = 0;
		this.controls.Gear = this.vehicle.Config.Power.NeutralIndex;
		
		this.changingGear = false;
		this.gearChangeDelta = 0;
		this.pressingClutch = false;
	}
	
	public override void UpdateControls() {
		this.controls.Throttle = InputManager.input.GetAxisAction("Throttle");
		this.controls.Brake = InputManager.input.GetAxisAction("Brake");
		this.controls.SteeringWheel = InputManager.input.GetAxisAction("SteeringWheel");
		UpdateGears();
		
		float clutchInc = Time.deltaTime / this.clutchPressTime;
		this.controls.Clutch = Math.Max(Math.Min(this.controls.Clutch + (this.pressingClutch ? clutchInc : -clutchInc), 1), 0);
		
		if (this.changingGear) {
			if (this.controls.Clutch == 1) { //Clutch fully pressed -> change gear
				this.controls.Gear += this.gearChangeDelta;
				this.gearChangeDelta = 0;
				this.pressingClutch = false;
			}
			if (controls.Clutch == 0) //Clutch fully released
				this.changingGear = false;
		}
		else {
			float maxPressRpm = this.pressingClutch ? this.releaseClutchRpm : this.pressClutchRpm;
			this.pressingClutch = this.vehicle.Props.EngineRpm < maxPressRpm;
		}
	}
	
	protected virtual void UpdateGears() {
		if (InputManager.input.GetButtonAction("GearUp"))
			ChangeGear(1);
		
		if (InputManager.input.GetButtonAction("GearDown"))
			ChangeGear(-1);
	}
	
	protected void ChangeGear(int gearDelta) {
		int newGear = this.controls.Gear + this.gearChangeDelta + gearDelta;
		
		if (newGear >= 0 && newGear < this.vehicle.Config.Power.GearsCount) {
			if (this.clutchPressTime > 0) {
				this.pressingClutch = true;
				this.changingGear = true;
				this.gearChangeDelta += gearDelta;
			}
			else
				this.controls.Gear += this.gearChangeDelta + gearDelta;
		}
	}
}