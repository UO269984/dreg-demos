using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

public class ConfigManager {
	
	internal IntPtr Ptr {get; private set;}
	private IntPtr vehicleConfigPtr;
	
	public VehicleConfig Config {get; private set;}
	
	public ConfigManager(bool createObjects) : this(Dreg.createConfigManager((char) (createObjects ? 1 : 0))) {}
	
	internal ConfigManager(IntPtr configManagerPtr) {
		this.Ptr = configManagerPtr;
		this.vehicleConfigPtr = Dreg.getVehicleConfig(this.Ptr);
		
		this.Config = new VehicleConfig();
		UpdateConfigStruct();
	}
	
	public void Delete() {
		Dreg.deleteConfigManager(this.Ptr);
	}
	
	public ConfigManager Clone(bool fullClone) {
		return new ConfigManager(Dreg.cloneConfig(this.Ptr, (char) (fullClone ? 1 : 0)));
	}
	
	public void Update() {
		Marshal.StructureToPtr(this.Config.Struct, this.vehicleConfigPtr, false);
		Dreg.updateConfig(this.Ptr);
	}
	
	public void LoadDefaultConfig() {
		Dreg.loadDefaultConfig(this.Ptr);
		UpdateConfigStruct();
	}
	
	public bool LoadSerializedConfig(String serializedConfig) {
		IntPtr configCharPtr = Marshal.StringToCoTaskMemAnsi(serializedConfig);
		bool loadSucess = Dreg.loadSerializedConfig(this.vehicleConfigPtr, configCharPtr) != 0;
		Marshal.FreeCoTaskMem(configCharPtr);
		
		if (loadSucess)
			UpdateConfigStruct();
		
		return loadSucess;
	}
	
	public String SerializeConfig() {
		IntPtr configCharPtr = Dreg.serializeConfig(this.vehicleConfigPtr);
		String serializedConfig = Marshal.PtrToStringAnsi(configCharPtr);
		Dreg.deleteCharArray(configCharPtr);
		
		return serializedConfig;
	}
	
	private void UpdateConfigStruct() {
		Marshal.PtrToStructure(this.vehicleConfigPtr, this.Config.Struct);
		this.Config.Update();
	}
}