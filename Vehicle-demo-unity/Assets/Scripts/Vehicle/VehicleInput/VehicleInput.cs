using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehicleInput : MonoBehaviour {
	
	protected Vehicle vehicle;
	protected VehicleControls controls;
	
	public void Configure(Vehicle vehicle, VehicleControls controls) {
		this.vehicle = vehicle;
		this.controls = controls;
	}
	
	public virtual void Reset() {}
	public abstract void UpdateControls();
}