using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleUI : VehicleUI {
	
	public HUD hud;
	public Transform[] steeringWheelsModels;
	
	public AudioSource engineAudio;
	public float rpmMaxPitch = 6000;
	
	public void OnEnable() {
		this.engineAudio.Play();
	}
	
	public void OnDisable() {
		this.engineAudio.Pause();
	}
	
	public override void UpdateUI(Vehicle vehicle, VehicleControls controls) {
		foreach (Transform wheel in this.steeringWheelsModels)
			wheel.localEulerAngles = new Vector3(0, 0, vehicle.Config.MaxSteeringAngle * controls.SteeringWheel);
		
		if (this.hud != null)
			this.hud.UpdateHUD(vehicle, controls);
		
		this.engineAudio.pitch = (vehicle.Props.EngineRpm / this.rpmMaxPitch) * 3;
	}
}