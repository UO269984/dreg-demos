using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleUI : VehicleUI {
	
	[System.Serializable]
	public struct WheelData {
		public Transform wheel;
		public bool isSteeringWheel;
	}
	
	public HUD hud;
	public WheelData[] wheels;
	
	public AudioSource engineAudio;
	public float rpmMaxPitch = 6000;
	private float wheelRotPos = 0;
	
	public void OnEnable() {
		this.engineAudio.Play();
	}
	
	public void OnDisable() {
		this.engineAudio.Pause();
	}
	
	public override void UpdateUI(Vehicle vehicle, VehicleControls controls) {
		this.wheelRotPos += ((vehicle.Props.WheelRpm / 60) * Time.deltaTime * 360);
		
		foreach (WheelData wheelData in this.wheels) {
			Quaternion rot = Quaternion.Euler(0, 0,
				wheelData.isSteeringWheel ? vehicle.Config.MaxSteeringAngle * controls.SteeringWheel : 0);
			
			wheelData.wheel.localRotation = rot * Quaternion.Euler(0, this.wheelRotPos, 0);
		}
		
		if (this.hud != null)
			this.hud.UpdateHUD(vehicle, controls);
		
		this.engineAudio.pitch = (vehicle.Props.EngineRpm / this.rpmMaxPitch) * 3;
	}
}