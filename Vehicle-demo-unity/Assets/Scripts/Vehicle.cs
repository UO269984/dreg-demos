using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

public abstract class Vehicle : MonoBehaviour {
	
	public Transform[] steeringWheelsModels;
	
	private IntPtr vehiclePtr;
	private IntPtr vehicleStatePtr;
	private IntPtr vehiclePropsPtr;
	private IntPtr vehicleConfigPtr;
	
	protected VehicleConfig vehicleConfig;
	
	protected VehicleControls controls;
	private IntPtr controlsPtr;
	
	public void Start() {
		this.vehiclePtr = DrivingEngine.createVehicle();
		this.vehicleStatePtr = DrivingEngine.getVehicleState(this.vehiclePtr);
		this.vehiclePropsPtr = DrivingEngine.getVehicleProps(this.vehiclePtr);
		this.vehicleConfigPtr = DrivingEngine.getVehicleConfig(this.vehiclePtr);
		
		this.vehicleConfig = new VehicleConfig();
		Marshal.PtrToStructure(this.vehicleConfigPtr, this.vehicleConfig);
		
		this.controlsPtr = Marshal.AllocHGlobal(Size.VehicleControls);
		this.controls = new VehicleControls();
		
		InitVehicle();
	}
	
	protected virtual void InitVehicle() {}
	
	protected void UpdateConfig() {
		Marshal.StructureToPtr(this.vehicleConfig, this.vehicleConfigPtr, false);
		DrivingEngine.updateVehicleConfig(this.vehiclePtr);
	}
	
	public void OnDestroy() {
		Marshal.FreeHGlobal(this.controlsPtr);
		DrivingEngine.deleteVehicle(this.vehiclePtr);
	}
	
	public IntPtr GetVehiclePtr() {
		return this.vehiclePtr;
	}
	
	public VehicleState GetVehicleState() {
		VehicleState vehicleState = new VehicleState();
		Marshal.PtrToStructure(this.vehicleStatePtr, vehicleState);
		
		return vehicleState;
	}
	
	public VehicleProps GetVehicleProps() {
		VehicleProps vehicleProps = new VehicleProps();
		Marshal.PtrToStructure(this.vehiclePropsPtr, vehicleProps);
		
		return vehicleProps;
	}
	
	public void Update() {
		UpdateControls();
		Marshal.StructureToPtr(this.controls, this.controlsPtr, false);
		DrivingEngine.setVehicleInput(this.vehiclePtr, this.controlsPtr);
		
		DrivingEngine.update(this.vehiclePtr, Time.deltaTime);
		UpdateVehicle();
	}
	
	protected abstract void UpdateControls();
	
	private void UpdateVehicle() {
		VehicleState vehicleState = GetVehicleState();
		
		transform.position = new Vector3(vehicleState.pos.x, transform.position.y, vehicleState.pos.y);
		transform.eulerAngles = vehicleState.rotation.toVector3();
		
		foreach (Transform wheel in this.steeringWheelsModels)
			wheel.localEulerAngles = new Vector3(0, 0, this.vehicleConfig.maxSteeringAngle * controls.steeringWheel);
	}
}