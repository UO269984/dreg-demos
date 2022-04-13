using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AOT;
using System.Runtime.InteropServices;

public class DrivingEngine : MonoBehaviour {
	
	#if WEBGL && ! UNITY_EDITOR
		public const String LIBRARY_NAME = "__Internal";
	
	#else
		public const String LIBRARY_NAME = "driving-engine";
	#endif
	
	public delegate void PrintFunc(IntPtr toPrint);
	
	[DllImport(LIBRARY_NAME)]
	public static extern void setPrintFunc(PrintFunc newPrintFunc);
	
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr createVehicle(IntPtr config);
	
	[DllImport(LIBRARY_NAME)]
	public static extern void deleteVehicle(IntPtr vehicle);
	
	[DllImport(LIBRARY_NAME)]
	public static extern void setVehicleInput(IntPtr vehicle, IntPtr input);
	
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr getVehicleState(IntPtr vehicle);
	
	[DllImport(LIBRARY_NAME)]
	public static extern void update(IntPtr vehicle, float delta);
	
	static DrivingEngine() {
		setPrintFunc(printFuncLog);
	}
	
	[MonoPInvokeCallback(typeof(PrintFunc))]
	private static void printFuncLog(IntPtr toPrint) {
		Debug.Log("Driving: " + Marshal.PtrToStringAnsi(toPrint));
	}
}

public class Size {
	public static int Vector3_Driving = 12;
	public static int VehicleConfig = 2 * Vector3_Driving + 4;
	public static int VehicleControls = 12;
	public static int VehicleState = 2 * Vector3_Driving;
}

[StructLayout(LayoutKind.Sequential, Pack=4, CharSet=CharSet.Auto)]
public class Vector3_Driving {
	public float x;
	public float y;
	public float z;
	
	public Vector3_Driving() {}
	public Vector3_Driving(Vector3 vector) {
		this.x = vector.x;
		this.y = vector.z;
		this.z = vector.y;
	}
	
	public Vector3 toVector3() {
		return new Vector3(this.x, this.z, this.y);
	}
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class VehicleConfig {
	public Vector3_Driving frontShaft;
	public Vector3_Driving rearShaft;
	public float maxSteeringAngle;
	
	public VehicleConfig(Vector3 frontShaft, Vector3 rearShaft, float maxSteeringAngle) {
		this.frontShaft = new Vector3_Driving(frontShaft);
		this.rearShaft = new Vector3_Driving(rearShaft);
		
		this.maxSteeringAngle = maxSteeringAngle;
	}
}

[StructLayout(LayoutKind.Sequential, Pack=4, CharSet=CharSet.Auto)]
public class VehicleControls {
	public float throttle;
	public float brake;
	public float steeringWheel;
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class VehicleState {
	public Vector3_Driving pos = new Vector3_Driving();
	public Vector3_Driving rotation = new Vector3_Driving();
}