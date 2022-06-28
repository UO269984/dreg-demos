using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralVehicle : Vehicle {
	
	private HUD hud;
	private Text brakeClutchTx;
	private Image brakeClutchBG;
	private bool brakeActive;
	
	public Transform frontShaft;
	public Transform rearShaft;
	public float maxSteeringAngle = 20;
	public float mass = 500;
	
	public float torqueToRpmAccel = 10;
	public float driveRatio = 1;
	public int neutralIndex = 0;
	public float[] gearRatios;
	
	protected override void InitVehicle() {
		this.hud = GetComponent<HUD>();
		this.hud.neutralIndex = this.neutralIndex;
		this.brakeClutchTx = this.hud.hudUI.transform.Find("BrakeClutchBG/BrakeClutchTx").GetComponent<Text>();
		this.brakeClutchBG = this.hud.hudUI.transform.Find("BrakeClutchBG").GetComponent<Image>();
		
		this.vehicleConfig.power.torqueToRpmAccel = this.torqueToRpmAccel;
		this.vehicleConfig.power.SetGearRatios(this.gearRatios);
		this.vehicleConfig.power.driveRatio = this.driveRatio;
		
		this.vehicleConfig.wheels.diameter = 0.5f;
		
		this.vehicleConfig.frontShaft = new Vector3_Driving(this.frontShaft.position - transform.position);
		this.vehicleConfig.rearShaft = new Vector3_Driving(this.rearShaft.position - transform.position);
		this.vehicleConfig.maxSteeringAngle = this.maxSteeringAngle;
		this.vehicleConfig.mass = this.mass;
		
		UpdateConfig();
		InitGraphs();
		AfterVehicleReset();
	}
	
	private void InitGraphs() {
		Graph.LoadLinearGraph(this.vehicleConfig.power.throttleCurve, new Vector2_Driving[] {
			new Vector2_Driving(0, 0.02f),
			new Vector2_Driving(0.05f, 0.02f),
			new Vector2_Driving(1, 1)});
		
		Graph.LoadLinearGraph(this.vehicleConfig.power.engineCurve, new Vector2_Driving[] {
			new Vector2_Driving(0, 50),
			new Vector2_Driving(1000, 500)});
		
		Graph.LoadLinearGraph(this.vehicleConfig.power.looseEngineRpmCurve, new Vector2_Driving[] {
			new Vector2_Driving(0, 0),
			new Vector2_Driving(1, 5000)});
		
		Graph.LoadLinearGraph(this.vehicleConfig.power.engineBrakeCurve, new Vector2_Driving[] {
			new Vector2_Driving(0, 0),
			new Vector2_Driving(250, -40),
			new Vector2_Driving(5000, -100)});
		
		Graph.LoadLinearGraph(this.vehicleConfig.power.clutchCurve, new Vector2_Driving[] {
			new Vector2_Driving(0, 600),
			new Vector2_Driving(1, 0)});
	}
	
	protected override void AfterVehicleReset() {
		this.controls.gear = this.neutralIndex;
		SetBrakeActive(true);
	}
	
	protected override void UpdateControls() {
		if (InputManager.input.GetAxisAction("ToggleBrakeClutch") > 0)
			SetBrakeActive(! this.brakeActive);
		
		this.controls.throttle = InputManager.input.GetAxisAction("Throttle");
		this.controls.brake = this.brakeActive ? InputManager.input.GetAxisAction("Brake") : 0;
		this.controls.steeringWheel = InputManager.input.GetAxisAction("SteeringWheel");
		this.controls.clutch = ! this.brakeActive ? InputManager.input.GetAxisAction("Clutch") : 0;
		
		if (InputManager.input.GetButtonAction("GearUp"))
			ChangeGear(1);
		
		if (InputManager.input.GetButtonAction("GearDown"))
			ChangeGear(-1);
	}
	
	private void ChangeGear(int gearDelta) {
		int newGear = this.controls.gear + gearDelta;
		if (newGear >= 0 && newGear < this.gearRatios.Length)
			this.controls.gear = newGear;
	}
	
	private void SetBrakeActive(bool active) {
		this.brakeActive = active;
		
		this.brakeClutchTx.text = this.brakeActive ? "B" : "C";
		this.brakeClutchBG.color = this.brakeActive ?
			this.hud.Transparent(this.hud.brakeColor) :
			this.hud.Transparent(this.hud.clutchColor);
	}
	
	protected override void AfterVehicleUpdate() {
		this.hud.UpdateHUD(this.controls, GetVehicleProps());
	}
}