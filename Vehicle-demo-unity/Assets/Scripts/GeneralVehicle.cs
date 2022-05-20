using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralVehicle : Vehicle {
	
	public Transform frontShaft;
	public Transform rearShaft;
	public float maxSteeringAngle = 20;
	
	protected override VehicleConfig getVehicleConfig(ref float maxSteeringAngle) {
		maxSteeringAngle = this.maxSteeringAngle;
		
		VehicleConfig config = new VehicleConfig(
			this.frontShaft.position - transform.position,
			this.rearShaft.position - transform.position,
			this.maxSteeringAngle);
		
		return config;
	}
	
	protected override void updateControls() {
		this.controls.throttle = InputManager.input.GetAxisAction("Throttle");
		this.controls.brake = InputManager.input.GetAxisAction("Brake");
		this.controls.steeringWheel = InputManager.input.GetAxisAction("SteeringWheel");
	}
}