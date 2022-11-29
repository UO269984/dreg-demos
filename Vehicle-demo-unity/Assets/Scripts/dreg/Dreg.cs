using System;

using System.Runtime.InteropServices;

public static class Dreg {
	
	#if WEBGL && ! UNITY_EDITOR
		public const String LIBRARY_NAME = "__Internal";
	#else
		public const String LIBRARY_NAME = "dreg";
	#endif
	
	public delegate void PrintFunc(IntPtr toPrint);
	public delegate void SaveFileFunc(IntPtr filename, IntPtr data);
	
	[DllImport(LIBRARY_NAME)]
	public static extern void setPrintFunc(PrintFunc newPrintFunc);
	[DllImport(LIBRARY_NAME)]
	public static extern void setSaveFileFunc(SaveFileFunc newSaveFileFunc);
	
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr createFloatList();
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr getFloatList(IntPtr list, IntPtr size);
	[DllImport(LIBRARY_NAME)]
	public static extern void setFloatList(IntPtr list, IntPtr values, UIntPtr valuesCount);
	[DllImport(LIBRARY_NAME)]
	public static extern void deleteFloatList(IntPtr list);
	[DllImport(LIBRARY_NAME)]
	public static extern void deleteCharArray(IntPtr array);
	
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
	public static extern IntPtr getConfigManager(IntPtr vehicle);
	[DllImport(LIBRARY_NAME)]
	public static extern void setVehicleConfig(IntPtr vehicle, IntPtr configManager);
	[DllImport(LIBRARY_NAME)]
	public static extern void update(IntPtr vehicle, float delta);
	
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr createConfigManager(char createObjects);
	[DllImport(LIBRARY_NAME)]
	public static extern void deleteConfigManager(IntPtr configManager);
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr cloneConfig(IntPtr configManager, char fullClone);
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr getVehicleConfig(IntPtr configManager);
	[DllImport(LIBRARY_NAME)]
	public static extern void updateConfig(IntPtr configManager);
	[DllImport(LIBRARY_NAME)]
	public static extern void loadDefaultConfig(IntPtr configManager);
	[DllImport(LIBRARY_NAME)]
	public static extern char loadSerializedConfig(IntPtr config, IntPtr serializedConfig);
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr serializeConfig(IntPtr config);
	
	[DllImport(LIBRARY_NAME)]
	public static extern void setGraphSaveInitData(char saveInitData);
	[DllImport(LIBRARY_NAME)]
	public static extern void setDefaultBezierSamples(UIntPtr samplesPerSegment);
	
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr createGraph();
	[DllImport(LIBRARY_NAME)]
	public static extern void deleteGraph(IntPtr graph);
	[DllImport(LIBRARY_NAME)]
	public static extern IntPtr cloneGraph(IntPtr graph);
	[DllImport(LIBRARY_NAME)]
	public static extern char loadLinearGraph(IntPtr graph, IntPtr refs, UIntPtr refsCount);
	[DllImport(LIBRARY_NAME)]
	public static extern char loadBezierGraph(IntPtr graph, IntPtr refs, UIntPtr refsCount, UIntPtr samplesPerSegment);
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
}

public class Size {
	public static int Char = Marshal.SizeOf(typeof(char));
	public static int Int = Marshal.SizeOf(typeof(int));
	public static int Float = Marshal.SizeOf(typeof(float));
	public static int Size_t = Marshal.SizeOf(typeof(UIntPtr));
	public static int Ptr = Marshal.SizeOf(typeof(IntPtr));
	public static int Vector2_Dreg = 2 * Float;
	public static int Vector3_Dreg = 3 * Float;
	public static int PowerConfig = 6 * Ptr + 2 * Float;
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
	public Vector2_Dreg(float x, float y) {
		this.x = x;
		this.y = y;
	}
	public Vector2_Dreg(Vector2_Dreg vector) {
		this.x = vector.x;
		this.y = vector.y;
	}
}

[StructLayout(LayoutKind.Sequential, Pack=0, CharSet=CharSet.Auto)]
public class Vector3_Dreg {
	public float x;
	public float y;
	public float z;
	
	public Vector3_Dreg() {}
	public Vector3_Dreg(float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public Vector3_Dreg(Vector3_Dreg vector) {
		this.x = vector.x;
		this.y = vector.y;
		this.z = vector.z;
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
	public IntPtr gearRatios;
	
	public float[] GetGearRatios() {
		IntPtr sizePtr = Marshal.AllocHGlobal(Size.Size_t);
		IntPtr array = Dreg.getFloatList(this.gearRatios, sizePtr);
		
		float[] ratios = new float[Marshal.ReadIntPtr(sizePtr).ToInt64()];
		if (ratios.Length != 0)
			Marshal.Copy(array, ratios, 0, ratios.Length);
		
		Marshal.FreeHGlobal(sizePtr);
		return ratios;
	}
	
	public void SetGearRatios(float[] newGearRatios) {
		IntPtr array = Marshal.AllocHGlobal(newGearRatios.Length * Size.Float);
		
		Marshal.Copy(newGearRatios, 0, array, newGearRatios.Length);
		Dreg.setFloatList(this.gearRatios, array, (UIntPtr) newGearRatios.Length);
		Marshal.FreeHGlobal(array);
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