using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralVehicle : AbstractVehicle {
	
	public GameObject brakeClutchUI;
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
		this.brakeClutchBG = this.brakeClutchUI.GetComponent<Image>();
		this.brakeClutchTx = this.brakeClutchUI.transform.GetChild(0).GetComponent<Text>();
		
		this.vehicle.Config.Power.TorqueToRpmAccel = this.torqueToRpmAccel;
		this.vehicle.Config.Power.GearRatios = this.gearRatios;
		this.vehicle.Config.Power.DriveRatio = this.driveRatio;
		
		this.vehicle.Config.Wheels.Diameter = 0.5f;
		
		this.vehicle.Config.FrontShaft = this.frontShaft.position - transform.position;
		this.vehicle.Config.RearShaft = this.rearShaft.position - transform.position;
		this.vehicle.Config.MaxSteeringAngle = this.maxSteeringAngle;
		this.vehicle.Config.Mass = this.mass;
		
		this.vehicle.UpdateConfig();
		InitGraphs();
		
		this.controls.Gear = this.neutralIndex;
		SetBrakeActive(true);
	}
	
	private void InitGraphs() {
		this.vehicle.Config.Power.ThrottleCurve.LoadLinearGraph(new Vector2[] {
			new Vector2(0, 0.02f),
			new Vector2(0.05f, 0.02f),
			new Vector2(1, 1)});
		
		this.vehicle.Config.Power.EngineCurve.LoadLinearGraph(new Vector2[] {
			new Vector2(0, 50),
			new Vector2(1000, 500)});
		
		this.vehicle.Config.Power.LooseEngineRpmCurve.LoadLinearGraph(new Vector2[] {
			new Vector2(0, 0),
			new Vector2(1, 5000)});
		
		this.vehicle.Config.Power.EngineBrakeCurve.LoadLinearGraph(new Vector2[] {
			new Vector2(0, 0),
			new Vector2(250, -40),
			new Vector2(5000, -100)});
		
		this.vehicle.Config.Power.ClutchCurve.LoadLinearGraph(new Vector2[] {
			new Vector2(0, 600),
			new Vector2(1, 0)});
	}
	
	public override void Reset() {
		base.Reset();
		this.controls.Gear = this.neutralIndex;
		SetBrakeActive(true);
	}
	
	protected override void UpdateControls() {
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
		if (newGear >= 0 && newGear < this.gearRatios.Length)
			this.controls.Gear = newGear;
	}
	
	private void SetBrakeActive(bool active) {
		this.brakeActive = active;
		
		this.brakeClutchTx.text = this.brakeActive ? "B" : "C";
		this.brakeClutchBG.color = this.brakeActive ?
			this.hud.Transparent(this.hud.brakeColor) :
			this.hud.Transparent(this.hud.clutchColor);
	}
	
	protected override void AfterVehicleUpdate() {
		this.hud.UpdateHUD(this.controls, this.vehicle.Props);
	}
}