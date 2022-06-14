using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AOT;
using System.Runtime.InteropServices;

public class DrivingEngine : MonoBehaviour {
	
	#if WEBGL && ! UNITY_EDITOR
		public const String LIBRARY_NAME = "__Internal";
		
		[DllImport(LIBRARY_NAME)]
		private static extern void downloadFileBrowser(IntPtr filename, IntPtr data);
	
	#else
		public const String LIBRARY_NAME = "driving-engine";
		
		private static void downloadFileBrowser(IntPtr filename, IntPtr data) {}
	#endif
	
	public delegate void PrintFunc(IntPtr toPrint);
	public delegate void SaveFileFunc(IntPtr filename, IntPtr data);
	
	[DllImport(LIBRARY_NAME)]
	public static extern void setPrintFunc(PrintFunc newPrintFunc);
	[DllImport(LIBRARY_NAME)]
	public static extern void setSaveFileFunc(SaveFileFunc newSaveFileFunc);
	
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr createVehicle();
	[DllImport(LIBRARY_NAME)]
	public static extern void deleteVehicle(IntPtr vehicle);
	[DllImport(LIBRARY_NAME)]
	public static extern void resetVehicle(IntPtr vehicle);
	
	[DllImport(LIBRARY_NAME)]
	public static extern void setVehicleInput(IntPtr vehicle, IntPtr input);
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr getVehicleState(IntPtr vehicle);
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr getVehicleProps(IntPtr vehicle);
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr getVehicleConfig(IntPtr vehicle);
	[DllImport(LIBRARY_NAME)]
	public static extern void updateVehicleConfig(IntPtr vehicle);
	[DllImport(LIBRARY_NAME)]
	public static extern void update(IntPtr vehicle, float delta);
	
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr createInputLogger(IntPtr vehicle);
	[DllImport(LIBRARY_NAME)]
	public static extern void logInput(IntPtr inputLogger, float delta);
	[DllImport(LIBRARY_NAME)]
	public static extern void saveInputLogger(IntPtr inputLogger, IntPtr filename);
	
	static DrivingEngine() {
		setPrintFunc(printFuncLog);
		
		if (Application.platform == RuntimePlatform.WebGLPlayer)
			setSaveFileFunc(saveFileBrowser);
	}
	
	[MonoPInvokeCallback(typeof(PrintFunc))]
	private static void printFuncLog(IntPtr toPrint) {
		Debug.Log("Driving: " + Marshal.PtrToStringAnsi(toPrint));
	}
	
	[MonoPInvokeCallback(typeof(SaveFileFunc))]
	private static void saveFileBrowser(IntPtr filename, IntPtr data) {
		downloadFileBrowser(filename, data);
	}
}

public class Size {
	public static int Char = Marshal.SizeOf(typeof(char));
	public static int Int = Marshal.SizeOf(typeof(int));
	public static int Float = Marshal.SizeOf(typeof(float));
	public static int Vector3_Driving = 3 * Float;
	public static int VehicleConfig = 2 * Vector3_Driving + Float;
	public static int VehicleControls = 3 * Float;
	public static int VehicleState = 2 * Vector3_Driving;
	public static int VehicleProps = 2 * Float;
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
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
	
	public VehicleConfig() {}
	public VehicleConfig(Vector3 frontShaft, Vector3 rearShaft, float maxSteeringAngle) {
		this.frontShaft = new Vector3_Driving(frontShaft);
		this.rearShaft = new Vector3_Driving(rearShaft);
		
		this.maxSteeringAngle = maxSteeringAngle;
	}
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
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

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class VehicleProps {
	public float speed;
	public float acceleration;
}