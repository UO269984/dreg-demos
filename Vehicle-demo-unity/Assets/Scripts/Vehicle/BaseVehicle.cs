using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVehicle : MonoBehaviour {
	
	public Vehicle Vehicle {get; private set;}
	private VehicleControls controls;
	
	private ConfigLoader configLoader;
	private VehicleInput vehicleInput;
	private VehicleUI vehicleUI;
	private bool initialized = false;
	
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
		
		this.vehicleUI = GetComponent<VehicleUI>();
		if (this.vehicleUI == null) {
			Debug.LogError("VehicleUI script not found");
			return;
		}
		this.initialized = true;
	}
	
	public void OnDestroy() {
		this.Vehicle.Delete();
	}
	
	public void OnEnable() {
		if (this.initialized) {
			this.configLoader.enabled = true;
			this.vehicleInput.enabled = true;
			this.vehicleUI.enabled = true;
		}
	}
	
	public void OnDisable() {
		if (this.initialized) {
			this.configLoader.enabled = false;
			this.vehicleInput.enabled = false;
			this.vehicleUI.enabled = false;
		}
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
		
		this.vehicleUI.UpdateUI(this.Vehicle, this.controls);
	}
}