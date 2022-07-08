using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractVehicle : MonoBehaviour {
	
	[HideInInspector]
	public Vehicle vehicle;
	protected VehicleControls controls;
	
	public Transform[] steeringWheelsModels;
	
	public void Start() {
		this.controls = new VehicleControls();
		this.vehicle = new Vehicle();
		InitVehicle();
	}
	
	protected virtual void InitVehicle() {}
	protected abstract void UpdateControls();
	protected virtual void AfterVehicleUpdate() {}
	
	public void OnDestroy() {
		this.vehicle.Delete();
	}
	
	public virtual void Reset() {
		this.vehicle.Reset();
	}
	
	public void Update() {
		UpdateControls();
		this.vehicle.SetVehicleInput(this.controls);
		
		this.vehicle.Update();
		this.vehicle.UpdateTransform(transform);
		foreach (Transform wheel in this.steeringWheelsModels)
			wheel.localEulerAngles = new Vector3(0, 0, this.vehicle.Config.MaxSteeringAngle * controls.SteeringWheel);
			
		AfterVehicleUpdate();
	}
}