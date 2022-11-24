using System;

using System.Runtime.InteropServices;

public class Vehicle {
	
	internal IntPtr Ptr {get; private set;}
	public ConfigManager ConfigManager {get; private set;}
	
	public VehicleConfig Config {get {return this.ConfigManager.Config;}}
	public VehicleState State {get; private set;}
	public VehicleProps Props {get; private set;}
	
	private IntPtr controlsPtr;
	
	public Vehicle() {
		this.Ptr = Dreg.createVehicle();
		
		this.State = new VehicleState(Dreg.getVehicleState(this.Ptr));
		this.Props = new VehicleProps(Dreg.getVehicleProps(this.Ptr));
		
		this.controlsPtr = Marshal.AllocHGlobal(Size.VehicleControls);
	}
	
	public void Delete() {
		Marshal.FreeHGlobal(this.controlsPtr);
		Dreg.deleteVehicle(this.Ptr);
	}
	
	public void Reset() {
		Dreg.resetVehicle(this.Ptr);
	}
	
	public void SetVehicleInput(VehicleControls controls) {
		Marshal.StructureToPtr(controls.Struct, this.controlsPtr, false);
		Dreg.setVehicleInput(this.Ptr, this.controlsPtr);
	}
	
	public void SetVehicleConfig(ConfigManager configManager) {
		this.ConfigManager = configManager;
		Dreg.setVehicleConfig(this.Ptr, configManager.Ptr);
	}
	
	public void Update(float delta) {
		Dreg.update(this.Ptr, delta);
		this.State.Invalidate();
		this.Props.Invalidate();
	}
}