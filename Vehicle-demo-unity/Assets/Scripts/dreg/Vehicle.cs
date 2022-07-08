using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Vehicle {
	
	private IntPtr vehiclePtr;
	private IntPtr vehicleConfigPtr;
	
	public VehicleConfig Config {get; private set;}
	public VehicleState State {get; private set;}
	public VehicleProps Props {get; private set;}
	
	private IntPtr controlsPtr;
	internal IntPtr VehiclePtr {get {return this.vehiclePtr;}}
	
	public Vehicle() {
		this.vehiclePtr = Dreg.createVehicle();
		this.vehicleConfigPtr = Dreg.getVehicleConfig(this.vehiclePtr);
		
		VehicleConfig_Struct configStruct = new VehicleConfig_Struct();
		Marshal.PtrToStructure(this.vehicleConfigPtr, configStruct);
		
		this.Config = new VehicleConfig(configStruct);
		this.State = new VehicleState(Dreg.getVehicleState(this.vehiclePtr));
		this.Props = new VehicleProps(Dreg.getVehicleProps(this.vehiclePtr));
		
		this.controlsPtr = Marshal.AllocHGlobal(Size.VehicleControls);
	}
	
	public void Delete() {
		Marshal.FreeHGlobal(this.controlsPtr);
		Dreg.deleteVehicle(this.vehiclePtr);
	}
	
	public void Reset() {
		Dreg.resetVehicle(this.vehiclePtr);
	}
	
	public void SetVehicleInput(VehicleControls controls) {
		Marshal.StructureToPtr(controls.Struct, this.controlsPtr, false);
		Dreg.setVehicleInput(this.vehiclePtr, this.controlsPtr);
	}
	
	public void UpdateConfig() {
		Marshal.StructureToPtr(this.Config.Struct, this.vehicleConfigPtr, false);
		Dreg.updateVehicleConfig(this.vehiclePtr);
	}
	
	public void Update() {
		Dreg.update(this.vehiclePtr, Time.deltaTime);
		this.State.Invalidate();
		this.Props.Invalidate();
	}
	
	public void UpdateTransform(Transform transform) {
		transform.position = new Vector3(this.State.Pos.x, transform.position.y, this.State.Pos.z);
		transform.eulerAngles = this.State.Rotation;
	}
}