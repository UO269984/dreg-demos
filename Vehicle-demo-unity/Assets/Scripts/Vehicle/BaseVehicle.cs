using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVehicle : MonoBehaviour {
	
	public Vehicle Vehicle {get; private set;}
	private VehicleControls controls;
	
	private ConfigLoader configLoader;
	private VehicleInput vehicleInput;
	
	public HUD hud;
	public Transform[] steeringWheelsModels;
	
	public void Start() {
		this.controls = new VehicleControls();
		this.Vehicle = new Vehicle();
		
		this.configLoader = GetComponent<ConfigLoader>();
		if (this.configLoader == null) {
			Debug.LogError("ConfigLoader script not found");
			return;
		}
		this.configLoader.LoadConfig(this.Vehicle.Config);
		this.Vehicle.UpdateConfig();
		
		this.vehicleInput = GetComponent<VehicleInput>();
		if (this.vehicleInput == null) {
			Debug.LogError("VehicleInput script not found");
			return;
		}
		this.vehicleInput.Configure(this.Vehicle, this.controls);
		this.vehicleInput.Reset();
	}
	
	public void OnDestroy() {
		this.Vehicle.Delete();
	}
	
	public void Reset() {
		this.Vehicle.Reset();
		this.vehicleInput.Reset();
	}
	
	public void Update() {
		this.vehicleInput.UpdateControls();
		this.Vehicle.SetVehicleInput(this.controls);
		
		this.Vehicle.Update();
		this.Vehicle.UpdateTransform(transform);
		
		foreach (Transform wheel in this.steeringWheelsModels)
			wheel.localEulerAngles = new Vector3(0, 0, this.Vehicle.Config.MaxSteeringAngle * controls.SteeringWheel);
		
		if (this.hud != null)
			this.hud.UpdateHUD(this.Vehicle, this.controls);
	}
}