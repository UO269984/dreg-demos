using System;

using System.Runtime.InteropServices;

public class PowerConfig {
	internal PowerConfig_Struct Struct;
	internal PowerConfig() {}
	
	internal void Update() {
		this.GearsCount = this.GearRatios.Length;
	}
	
	public Graph ThrottleCurve {
		get {return Graph.FromPtr(this.Struct.throttleCurve);}
		set {this.Struct.throttleCurve = value.graphPtr;}
	}
	
	public Graph EngineCurve {
		get {return Graph.FromPtr(this.Struct.engineCurve);}
		set {this.Struct.engineCurve = value.graphPtr;}
	}
	
	public Graph LooseEngineRpmCurve {
		get {return Graph.FromPtr(this.Struct.looseEngineRpmCurve);}
		set {this.Struct.looseEngineRpmCurve = value.graphPtr;}
	}
	
	public Graph EngineBrakeCurve {
		get {return Graph.FromPtr(this.Struct.engineBrakeCurve);}
		set {this.Struct.engineBrakeCurve = value.graphPtr;}
	}
	
	public Graph ClutchCurve {
		get {return Graph.FromPtr(this.Struct.clutchCurve);}
		set {this.Struct.clutchCurve = value.graphPtr;}
	}
	
	public float TorqueToRpmAccel {
		get {return this.Struct.torqueToRpmAccel;}
		set {this.Struct.torqueToRpmAccel = value;}
	}
	
	public float DriveRatio {
		get {return this.Struct.driveRatio;}
		set {this.Struct.driveRatio = value;}
	}
	
	public int NeutralIndex {get; set;}
	public int GearsCount {get; private set;}
	
	public float[] GearRatios {
		get {return this.Struct.GetGearRatios();}
		set {
			this.Struct.SetGearRatios(value);
			this.GearsCount = value.Length;
		}
	}
}

public class WheelConfig {
	internal WheelConfig_Struct Struct;
	internal WheelConfig() {}
	
	public float Diameter {
		get {return this.Struct.diameter;}
		set {this.Struct.diameter = value;}
	}
	
	public float BrakeDiameter {
		get {return this.Struct.brakeDiameter;}
		set {this.Struct.brakeDiameter = value;}
	}
	
	public float BrakeStaticFrictionCoef {
		get {return this.Struct.brakeStaticFrictionCoef;}
		set {this.Struct.brakeStaticFrictionCoef = value;}
	}
	
	public float BrakeKineticFrictionCoef {
		get {return this.Struct.brakeKineticFrictionCoef;}
		set {this.Struct.brakeKineticFrictionCoef = value;}
	}
}

public class VehicleConfig {
	internal VehicleConfig_Struct Struct {get; private set;}
	public PowerConfig Power {get; private set;}
	public WheelConfig Wheels {get; private set;}
	
	internal VehicleConfig() {
		this.Struct = new VehicleConfig_Struct();
		this.Power = new PowerConfig();
		this.Wheels = new WheelConfig();
	}
	
	internal void Update() {
		//Reloading config with Marshal.PtrToStructure changes Power and Wheel objects in Struct object
		//We have to reload the Power and Wheels references
		this.Power.Struct = this.Struct.power;
		this.Wheels.Struct = this.Struct.wheels;
		this.Power.Update();
	}
	
	public Vector3_Dreg FrontShaft {
		get {return this.Struct.frontShaft;}
		set {this.Struct.frontShaft = value;}
	}
	
	public Vector3_Dreg RearShaft {
		get {return this.Struct.rearShaft;}
		set {this.Struct.rearShaft = value;}
	}
	
	public float MaxSteeringAngle {
		get {return this.Struct.maxSteeringAngle;}
		set {this.Struct.maxSteeringAngle = value;}
	}
	
	public float Mass {
		get {return this.Struct.mass;}
		set {this.Struct.mass = value;}
	}
	
	public Graph BrakeCurve {
		get {return Graph.FromPtr(this.Struct.brakeCurve);}
		set {this.Struct.brakeCurve = value.graphPtr;}
	}
}

public class VehicleControls {
	internal VehicleControls_Struct Struct {get; private set;}
	
	public VehicleControls() {
		this.Struct = new VehicleControls_Struct();
	}
	
	internal VehicleControls(VehicleControls_Struct controls) {
		this.Struct = controls;
	}
	
	public float Throttle {
		get {return this.Struct.throttle;}
		set {this.Struct.throttle = value;}
	}
	
	public float Brake {
		get {return this.Struct.brake;}
		set {this.Struct.brake = value;}
	}
	
	public float SteeringWheel {
		get {return this.Struct.steeringWheel;}
		set {this.Struct.steeringWheel = value;}
	}
	
	public float Clutch {
		get {return this.Struct.clutch;}
		set {this.Struct.clutch = value;}
	}
	
	public int Gear {
		get {return this.Struct.gear;}
		set {this.Struct.gear = value;}
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
	
	internal T GetObj() {
		if (! this.valid)
			UpdateObj();
		
		return this.obj;
	}
	
	private void UpdateObj() {
		Marshal.PtrToStructure(this.ptr, this.obj);
		this.valid = true;
	}
	
	internal void Invalidate() {
		this.valid = false;
	}
}

public class VehicleState : PtrToObjConverter<VehicleState_Struct> {
	internal VehicleState(IntPtr ptr) : base(ptr) {}
	
	public Vector3_Dreg Pos {get {return GetObj().pos;}}
	public Vector3_Dreg Rotation {get {return GetObj().rotation;}}
}

public class VehicleProps : PtrToObjConverter<VehicleProps_Struct> {
	internal VehicleProps(IntPtr ptr) : base(ptr) {}
	
	public float Speed {get {return GetObj().speed;}}
	public float Acceleration {get {return GetObj().acceleration;}}
	public float WheelRpm {get {return GetObj().wheelRpm;}}
	public float EngineRpm {get {return GetObj().engineRpm;}}
	public float EngineTorque {get {return GetObj().engineTorque;}}
	public float ClutchTorque {get {return GetObj().clutchTorque;}}
	public float PowerTorque {get {return GetObj().powerTorque;}}
	public float BrakeTorque {get {return GetObj().brakeTorque;}}
	public float WheelTorque {get {return GetObj().wheelTorque;}}
}