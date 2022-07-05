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
		this.vehiclePtr = Dreg.createVehicle();
		this.vehicleStatePtr = Dreg.getVehicleState(this.vehiclePtr);
		this.vehiclePropsPtr = Dreg.getVehicleProps(this.vehiclePtr);
		this.vehicleConfigPtr = Dreg.getVehicleConfig(this.vehiclePtr);
		
		this.vehicleConfig = new VehicleConfig();
		Marshal.PtrToStructure(this.vehicleConfigPtr, this.vehicleConfig);
		
		this.controlsPtr = Marshal.AllocHGlobal(Size.VehicleControls);
		this.controls = new VehicleControls();
		
		InitVehicle();
	}
	
	protected virtual void InitVehicle() {}
	protected virtual void AfterVehicleReset() {}
	
	protected void UpdateConfig() {
		Marshal.StructureToPtr(this.vehicleConfig, this.vehicleConfigPtr, false);
		Dreg.updateVehicleConfig(this.vehiclePtr);
	}
	
	public void OnDestroy() {
		Marshal.FreeHGlobal(this.controlsPtr);
		Dreg.deleteVehicle(this.vehiclePtr);
	}
	
	public void Reset() {
		Dreg.resetVehicle(this.vehiclePtr);
		AfterVehicleReset();
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
		Dreg.setVehicleInput(this.vehiclePtr, this.controlsPtr);
		
		Dreg.update(this.vehiclePtr, Time.deltaTime);
		UpdateVehicle();
		AfterVehicleUpdate();
	}
	
	protected abstract void UpdateControls();
	protected virtual void AfterVehicleUpdate() {}
	
	private void UpdateVehicle() {
		VehicleState vehicleState = GetVehicleState();
		
		transform.position = new Vector3(vehicleState.pos.x, transform.position.y, vehicleState.pos.y);
		transform.eulerAngles = vehicleState.rotation.toVector3();
		
		foreach (Transform wheel in this.steeringWheelsModels)
			wheel.localEulerAngles = new Vector3(0, 0, this.vehicleConfig.maxSteeringAngle * controls.steeringWheel);
	}
}