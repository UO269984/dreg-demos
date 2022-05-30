using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

public abstract class Vehicle : MonoBehaviour {
	
	public Transform[] steeringWheelsModels;
	
	private float maxSteeringAngleConf;
	
	private IntPtr vehiclePtr;
	private IntPtr vehicleStatePtr;
	
	protected VehicleControls controls;
	private IntPtr controlsPtr;
	
	public void Start() {
		InitVehicle();
		
		IntPtr configPtr = Marshal.AllocHGlobal(Size.VehicleConfig);
		Marshal.StructureToPtr(getVehicleConfig(ref this.maxSteeringAngleConf), configPtr, false);
		
		this.vehiclePtr = DrivingEngine.createVehicle(configPtr);
		this.vehicleStatePtr = DrivingEngine.getVehicleState(this.vehiclePtr);
		
		Marshal.FreeHGlobal(configPtr);
		
		this.controlsPtr = Marshal.AllocHGlobal(Size.VehicleControls);
		this.controls = new VehicleControls();
	}
	
	protected virtual void InitVehicle() {}
	protected abstract VehicleConfig getVehicleConfig(ref float maxSteeringAngle);
	
	public void OnDestroy() {
		Marshal.FreeHGlobal(this.controlsPtr);
		DrivingEngine.deleteVehicle(this.vehiclePtr);
	}
	
	public IntPtr getVehiclePtr() {
		return this.vehiclePtr;
	}
	
	public void Update() {
		updateControls();
		Marshal.StructureToPtr(this.controls, this.controlsPtr, false);
		DrivingEngine.setVehicleInput(this.vehiclePtr, this.controlsPtr);
		
		DrivingEngine.update(this.vehiclePtr, Time.deltaTime);
		updateVehicle();
	}
	
	protected abstract void updateControls();
	
	private void updateVehicle() {
		VehicleState vehicleState = new VehicleState();
		Marshal.PtrToStructure(this.vehicleStatePtr, vehicleState);
		
		transform.position = new Vector3(vehicleState.pos.x, transform.position.y, vehicleState.pos.y);
		transform.eulerAngles = vehicleState.rotation.toVector3();
		
		foreach (Transform wheel in this.steeringWheelsModels)
			wheel.localEulerAngles = new Vector3(0, 0, this.maxSteeringAngleConf * controls.steeringWheel);
	}
}