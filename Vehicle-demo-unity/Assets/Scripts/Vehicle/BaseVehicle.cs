using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVehicle : MonoBehaviour {
	
	public Vehicle Vehicle {get; private set;}
	private VehicleControls controls;
	
	public ConfigManagerScript configManagerScript;
	private VehicleInput vehicleInput;
	private VehicleUI vehicleUI;
	private bool initialized = false;
	
	public void Start() {
		this.controls = new VehicleControls();
		this.Vehicle = new Vehicle();
		LoadConfig();
		
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
			this.vehicleInput.enabled = true;
			this.vehicleUI.enabled = true;
		}
	}
	
	public void OnDisable() {
		if (this.initialized) {
			this.vehicleInput.enabled = false;
			this.vehicleUI.enabled = false;
		}
	}
	
	public void LoadConfig() {
		this.Vehicle.SetVehicleConfig(this.configManagerScript.LoadConfig());
	}
	
	public void Reset() {
		this.Vehicle.Reset();
		this.vehicleInput.Reset();
	}
	
	public void Update() {
		this.vehicleInput.UpdateControls();
		this.Vehicle.SetVehicleInput(this.controls);
		
		this.Vehicle.Update(Time.deltaTime);
		UpdateTransform();
		
		this.vehicleUI.UpdateUI(this.Vehicle, this.controls);
	}
	
	private void UpdateTransform() {
		transform.position = new Vector3_Dreg(
			this.Vehicle.State.Pos.x,
			this.Vehicle.State.Pos.y,
			transform.position.y).ToUnityVec();
		
		transform.eulerAngles = this.Vehicle.State.Rotation.ToUnityVec();
	}
}