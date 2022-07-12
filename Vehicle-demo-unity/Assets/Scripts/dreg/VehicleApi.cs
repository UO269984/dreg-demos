using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PowerConfig {
	private PowerConfig_Struct powerConfig;
	
	internal PowerConfig(PowerConfig_Struct powerConfig) {
		this.powerConfig = powerConfig;
	}
	
	public Graph ThrottleCurve {
		get {return Graph.FromPtr(this.powerConfig.throttleCurve);}
		set {this.powerConfig.throttleCurve = value.graphPtr;}
	}
	
	public Graph EngineCurve {
		get {return Graph.FromPtr(this.powerConfig.engineCurve);}
		set {this.powerConfig.engineCurve = value.graphPtr;}
	}
	
	public Graph LooseEngineRpmCurve {
		get {return Graph.FromPtr(this.powerConfig.looseEngineRpmCurve);}
		set {this.powerConfig.looseEngineRpmCurve = value.graphPtr;}
	}
	
	public Graph EngineBrakeCurve {
		get {return Graph.FromPtr(this.powerConfig.engineBrakeCurve);}
		set {this.powerConfig.engineBrakeCurve = value.graphPtr;}
	}
	
	public Graph ClutchCurve {
		get {return Graph.FromPtr(this.powerConfig.clutchCurve);}
		set {this.powerConfig.clutchCurve = value.graphPtr;}
	}
	
	public float TorqueToRpmAccel {
		get {return this.powerConfig.torqueToRpmAccel;}
		set {this.powerConfig.torqueToRpmAccel = value;}
	}
	
	public float DriveRatio {
		get {return this.powerConfig.driveRatio;}
		set {this.powerConfig.driveRatio = value;}
	}
	
	public int NeutralIndex {get; set;}
	public int GearsCount {
		get {return this.powerConfig.gearsCount;}
	}
	
	public float[] GearRatios {
		get {return this.powerConfig.GetGearRatios();}
		set {this.powerConfig.SetGearRatios(value);}
	}
}

public class WheelConfig {
	private WheelConfig_Struct wheelConfig;
	
	internal WheelConfig(WheelConfig_Struct wheelConfig) {
		this.wheelConfig = wheelConfig;
	}
	
	public float Diameter {
		get {return this.wheelConfig.diameter;}
		set {this.wheelConfig.diameter = value;}
	}
}

public class VehicleConfig {
	private VehicleConfig_Struct config;
	internal VehicleConfig_Struct Struct {get {return this.config;}}
	public PowerConfig Power {get; private set;}
	public WheelConfig Wheels {get; private set;}
	
	internal VehicleConfig(VehicleConfig_Struct config) {
		this.config = config;
		this.Power = new PowerConfig(this.config.power);
		this.Wheels = new WheelConfig(this.config.wheels);
	}
	
	public Vector3 FrontShaft {
		get {return this.config.frontShaft.ToVector3();}
		set {this.config.frontShaft = new Vector3_Dreg(value);}
	}
	
	public Vector3 RearShaft {
		get {return this.config.rearShaft.ToVector3();}
		set {this.config.rearShaft = new Vector3_Dreg(value);}
	}
	
	public float MaxSteeringAngle {
		get {return this.config.maxSteeringAngle;}
		set {this.config.maxSteeringAngle = value;}
	}
	
	public float Mass {
		get {return this.config.mass;}
		set {this.config.mass = value;}
	}
}

public class VehicleControls {
	private VehicleControls_Struct controls;
	internal VehicleControls_Struct Struct {get {return this.controls;}}
	
	public VehicleControls() {
		this.controls = new VehicleControls_Struct();
	}
	
	internal VehicleControls(VehicleControls_Struct controls) {
		this.controls = controls;
	}
	
	public float Throttle {
		get {return this.controls.throttle;}
		set {this.controls.throttle = value;}
	}
	
	public float Brake {
		get {return this.controls.brake;}
		set {this.controls.brake = value;}
	}
	
	public float SteeringWheel {
		get {return this.controls.steeringWheel;}
		set {this.controls.steeringWheel = value;}
	}
	
	public float Clutch {
		get {return this.controls.clutch;}
		set {this.controls.clutch = value;}
	}
	
	public int Gear {
		get {return this.controls.gear;}
		set {this.controls.gear = value;}
	}
}

public class PtrToObjConverter<T> where T : new() {
	private T obj;
	private IntPtr ptr;
	private bool valid = false;
	
	internal PtrToObjConverter(IntPtr ptr) {
		this.ptr = ptr;
		this.obj = new T();
	}
	
	public T GetObj() {
		if (! this.valid)
			UpdateObj();
		
		return this.obj;
	}
	
	public void UpdateObj() {
		Marshal.PtrToStructure(this.ptr, this.obj);
		this.valid = true;
	}
	
	public void Invalidate() {
		this.valid = false;
	}
}

public class VehicleState : PtrToObjConverter<VehicleState_Struct> {
	internal VehicleState(IntPtr ptr) : base(ptr) {}
	
	public Vector3 Pos {get {return GetObj().pos.ToVector3();}}
	public Vector3 Rotation {get {return GetObj().rotation.ToVector3();}}
}

public class VehicleProps : PtrToObjConverter<VehicleProps_Struct> {
	internal VehicleProps(IntPtr ptr) : base(ptr) {}
	
	public float Speed {get {return GetObj().speed;}}
	public float Acceleration {get {return GetObj().acceleration;}}
	public float WheelRpm {get {return GetObj().wheelRpm;}}
	public float EngineRpm {get {return GetObj().engineRpm;}}
	public float EngineTorque {get {return GetObj().engineTorque;}}
	public float ClutchTorque {get {return GetObj().clutchTorque;}}
	public float AxleShaftTorque {get {return GetObj().axleShaftTorque;}}
}