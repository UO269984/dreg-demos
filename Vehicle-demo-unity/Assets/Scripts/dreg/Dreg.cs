using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AOT;
using System.Runtime.InteropServices;

public class Dreg {
	
	#if WEBGL && ! UNITY_EDITOR
		public const String LIBRARY_NAME = "__Internal";
		
		[DllImport(LIBRARY_NAME)]
		private static extern void downloadFileBrowser(IntPtr filename, IntPtr data);
	
	#else
		public const String LIBRARY_NAME = "dreg";
		
		private static void downloadFileBrowser(IntPtr filename, IntPtr data) {}
	#endif
	
	public delegate void PrintFunc(IntPtr toPrint);
	public delegate void SaveFileFunc(IntPtr filename, IntPtr data);
	
	[DllImport(LIBRARY_NAME)]
	public static extern void setPrintFunc(PrintFunc newPrintFunc);
	[DllImport(LIBRARY_NAME)]
	public static extern void setSaveFileFunc(SaveFileFunc newSaveFileFunc);
	
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr createFloatArray(UIntPtr size);
	[DllImport(LIBRARY_NAME)]
	public static extern void deleteFloatArray(IntPtr array);
	
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
	public static extern IntPtr createGraph();
	[DllImport(LIBRARY_NAME)]
	public static extern void deleteGraph(IntPtr graph);
	[DllImport(LIBRARY_NAME)]
	public static extern void loadLinearGraph(IntPtr graph, IntPtr refs, UIntPtr refsCount);
	[DllImport(LIBRARY_NAME)]
	public static extern void loadBezierGraph(IntPtr graph, IntPtr refs, UIntPtr refsCount, UIntPtr samplesPerSegment);
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr getGraphPoints(IntPtr graph, IntPtr pointsCount);
	[DllImport(LIBRARY_NAME)]
	public static extern float getGraphY(IntPtr graph, float x);
	
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr createInputLogger(IntPtr vehicle);
	[DllImport(LIBRARY_NAME)]
	public static extern void logInput(IntPtr inputLogger, float delta);
	[DllImport(LIBRARY_NAME)]
	public static extern void saveInputLogger(IntPtr inputLogger, IntPtr filename);
	
	static Dreg() {
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
	public static int Size_t = Marshal.SizeOf(typeof(UIntPtr));
	public static int Ptr = Marshal.SizeOf(typeof(IntPtr));
	public static int Vector2_Dreg = 2 * Float;
	public static int Vector3_Dreg = 3 * Float;
	public static int PowerConfig = 6 * Ptr + 2 * Float + Int;
	public static int WheelConfig = Float;
	public static int VehicleConfig = 2 * Vector3_Dreg + 2 * Float + PowerConfig + WheelConfig;
	public static int VehicleControls = 4 * Float + Int;
	public static int VehicleState = 2 * Vector3_Dreg;
	public static int VehicleProps = 7 * Float;
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class Vector2_Dreg {
	public float x;
	public float y;
	
	public Vector2_Dreg() {}
	public Vector2_Dreg(Vector2 vector) {
		this.x = vector.x;
		this.y = vector.y;
	}
	
	public Vector2 ToVector2() {
		return new Vector2(this.x, this.y);
	}
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class Vector3_Dreg {
	public float x;
	public float y;
	public float z;
	
	public Vector3_Dreg() {}
	public Vector3_Dreg(Vector3 vector) {
		this.x = vector.x;
		this.y = vector.z;
		this.z = vector.y;
	}
	
	public Vector3 ToVector3() {
		return new Vector3(this.x, this.z, this.y);
	}
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class PowerConfig_Struct {
	public IntPtr throttleCurve;
	public IntPtr engineCurve;
	public IntPtr looseEngineRpmCurve;
	public IntPtr engineBrakeCurve;
	public IntPtr clutchCurve;
	
	public float torqueToRpmAccel;
	public float driveRatio;
	public int gearsCount;
	public IntPtr gearRatios;
	
	public float[] GetGearRatios() {
		float[] ratios = new float[this.gearsCount];
		Marshal.Copy(this.gearRatios, ratios, 0, this.gearsCount);
		return ratios;
	}
	
	public void SetGearRatios(float[] newGearRatios) {
		this.gearsCount = newGearRatios.Length;
		
		Dreg.deleteFloatArray(this.gearRatios);
		this.gearRatios = Dreg.createFloatArray((UIntPtr) this.gearsCount);
		Marshal.Copy(newGearRatios, 0, this.gearRatios, this.gearsCount);
	}
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class WheelConfig_Struct {
	public float diameter;
	
	public float brakeDiameter;
	public float brakeStaticFrictionCoef;
	public float brakeKineticFrictionCoef;
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class VehicleConfig_Struct {
	public Vector3_Dreg frontShaft;
	public Vector3_Dreg rearShaft;
	
	public float maxSteeringAngle;
	public float mass;
	
	public IntPtr brakeCurve;
	public PowerConfig_Struct power;
	public WheelConfig_Struct wheels;
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class VehicleControls_Struct {
	public float throttle;
	public float brake;
	public float steeringWheel;
	
	public float clutch;
	public int gear;
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class VehicleState_Struct {
	public Vector3_Dreg pos = new Vector3_Dreg();
	public Vector3_Dreg rotation = new Vector3_Dreg();
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class VehicleProps_Struct {
	public float speed;
	public float acceleration;
	
	public float wheelRpm;
	public float engineRpm;
	public float engineTorque;
	public float clutchTorque;
	
	public float powerTorque;
	public float brakeTorque;
	public float wheelTorque;
}