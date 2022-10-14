using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

public enum GamepadBt {
	Lx, Ly,
	Rx, Ry,
	X, Circle, Square, Triangle,
	CrossX, CrossY,
	L1, R1,
	L2, R2,
	L3, R3,
	Options, Share
}
	
public class GamepadInput : IInput {
	private static IDictionary<GamepadBt, KeyCode> buttonsMappingLinux = new Dictionary<GamepadBt, KeyCode>() {
		{GamepadBt.X, KeyCode.JoystickButton0},
		{GamepadBt.Circle, KeyCode.JoystickButton1},
		{GamepadBt.Square, KeyCode.JoystickButton2},
		{GamepadBt.Triangle, KeyCode.JoystickButton3},
		{GamepadBt.L1, KeyCode.JoystickButton4},
		{GamepadBt.R1, KeyCode.JoystickButton5},
		{GamepadBt.L3, KeyCode.JoystickButton9},
		{GamepadBt.R3, KeyCode.JoystickButton10},
		{GamepadBt.Options, KeyCode.JoystickButton7},
		{GamepadBt.Share, KeyCode.JoystickButton6}
	};
	private static IDictionary<GamepadBt, Tuple<String, Func<float, float>>> axisMappingLinux = new Dictionary<GamepadBt, Tuple<String, Func<float, float>>>() {
		{GamepadBt.Lx, getStandardAxis("X")},
		{GamepadBt.Ly, getStandardAxis("Y")},
		{GamepadBt.Rx, getStandardAxis("4")},
		{GamepadBt.Ry, getStandardAxis("5")},
		{GamepadBt.L2, getStandardAxis("3")},
		{GamepadBt.R2, getStandardAxis("6")},
		{GamepadBt.CrossX, getStandardAxis("7")},
		{GamepadBt.CrossY, getStandardAxis("8")}
	};
	private static IDictionary<GamepadBt, KeyCode> buttonsMappingWindows = new Dictionary<GamepadBt, KeyCode>() {
		{GamepadBt.X, KeyCode.JoystickButton1},
		{GamepadBt.Circle, KeyCode.JoystickButton2},
		{GamepadBt.Square, KeyCode.JoystickButton0},
		{GamepadBt.Triangle, KeyCode.JoystickButton3},
		{GamepadBt.L1, KeyCode.JoystickButton4},
		{GamepadBt.R1, KeyCode.JoystickButton5},
		{GamepadBt.L3, KeyCode.JoystickButton10},
		{GamepadBt.R3, KeyCode.JoystickButton11},
		{GamepadBt.Options, KeyCode.JoystickButton9},
		{GamepadBt.Share, KeyCode.JoystickButton8}
	};
	private static IDictionary<GamepadBt, Tuple<String, Func<float, float>>> axisMappingWindows = new Dictionary<GamepadBt, Tuple<String, Func<float, float>>>() {
		{GamepadBt.Lx, getStandardAxis("X")},
		{GamepadBt.Ly, getStandardAxis("Y")},
		{GamepadBt.Rx, getStandardAxis("3")},
		{GamepadBt.Ry, getStandardAxis("6")},
		{GamepadBt.L2, getAxis0Centered("4")},
		{GamepadBt.R2, getAxis0Centered("5")},
		{GamepadBt.CrossX, getStandardAxis("7")},
		{GamepadBt.CrossY, getStandardAxis("8")}
	};
	
	private static IDictionary<GamepadBt, KeyCode> buttonsMappingChrome = new Dictionary<GamepadBt, KeyCode>() {
		{GamepadBt.X, KeyCode.JoystickButton0},
		{GamepadBt.Circle, KeyCode.JoystickButton1},
		{GamepadBt.Square, KeyCode.JoystickButton2},
		{GamepadBt.Triangle, KeyCode.JoystickButton3},
		{GamepadBt.L1, KeyCode.JoystickButton4},
		{GamepadBt.R1, KeyCode.JoystickButton5},
		{GamepadBt.L3, KeyCode.JoystickButton8},
		{GamepadBt.R3, KeyCode.JoystickButton9},
		{GamepadBt.Options, KeyCode.JoystickButton7},
		{GamepadBt.Share, KeyCode.JoystickButton6}
	};
	private static IDictionary<GamepadBt, Tuple<String, Func<float, float>>> axisMappingChrome = new Dictionary<GamepadBt, Tuple<String, Func<float, float>>>() {
		{GamepadBt.Lx, getStandardAxis("X")},
		{GamepadBt.Ly, getStandardAxis("Y")},
		{GamepadBt.Rx, getStandardAxis("4")},
		{GamepadBt.Ry, getStandardAxis("5")},
		{GamepadBt.L2, getStandardAxis("9")},
		{GamepadBt.R2, getStandardAxis("10")},
		{GamepadBt.CrossX, getStandardAxis("6")},
		{GamepadBt.CrossY, getStandardAxis("7")}
	};
	private static IDictionary<GamepadBt, KeyCode> buttonsMappingFirefoxLinux = new Dictionary<GamepadBt, KeyCode>() {
		{GamepadBt.X, KeyCode.JoystickButton0},
		{GamepadBt.Circle, KeyCode.JoystickButton1},
		{GamepadBt.Square, KeyCode.JoystickButton3},
		{GamepadBt.Triangle, KeyCode.JoystickButton2},
		{GamepadBt.L1, KeyCode.JoystickButton4},
		{GamepadBt.R1, KeyCode.JoystickButton5},
		{GamepadBt.L3, KeyCode.JoystickButton11},
		{GamepadBt.R3, KeyCode.JoystickButton12},
		{GamepadBt.Options, KeyCode.JoystickButton9},
		{GamepadBt.Share, KeyCode.JoystickButton8}
	};
	private static IDictionary<GamepadBt, Tuple<String, Func<float, float>>> axisMappingFirefoxLinux = new Dictionary<GamepadBt, Tuple<String, Func<float, float>>>() {
		{GamepadBt.Lx, getStandardAxis("X")},
		{GamepadBt.Ly, getStandardAxis("Y")},
		{GamepadBt.Rx, getStandardAxis("4")},
		{GamepadBt.Ry, getStandardAxis("5")},
		{GamepadBt.L2, getAxis0Centered("3")},
		{GamepadBt.R2, getAxis0Centered("6")},
		{GamepadBt.CrossX, getStandardAxis("7")},
		{GamepadBt.CrossY, getStandardAxis("8")}
	};
	private static IDictionary<GamepadBt, KeyCode> buttonsMappingFirefoxWindows = new Dictionary<GamepadBt, KeyCode>() {
		{GamepadBt.X, KeyCode.JoystickButton0},
		{GamepadBt.Circle, KeyCode.JoystickButton1},
		{GamepadBt.Square, KeyCode.JoystickButton2},
		{GamepadBt.Triangle, KeyCode.JoystickButton3},
		{GamepadBt.L1, KeyCode.JoystickButton4},
		{GamepadBt.R1, KeyCode.JoystickButton5},
		{GamepadBt.L3, KeyCode.JoystickButton8},
		{GamepadBt.R3, KeyCode.JoystickButton9},
		{GamepadBt.Options, KeyCode.JoystickButton7},
		{GamepadBt.Share, KeyCode.JoystickButton6}
	};
	private static IDictionary<GamepadBt, Tuple<String, Func<float, float>>> axisMappingFirefoxWindows = new Dictionary<GamepadBt, Tuple<String, Func<float, float>>>() {
		{GamepadBt.Lx, getStandardAxis("X")},
		{GamepadBt.Ly, getStandardAxis("Y")},
		{GamepadBt.Rx, getStandardAxis("4")},
		{GamepadBt.Ry, getStandardAxis("5")},
		{GamepadBt.L2, getAxis0Centered("9")},
		{GamepadBt.R2, getAxis0Centered("10")},
		{GamepadBt.CrossX, getStandardAxis("6")},
		{GamepadBt.CrossY, getStandardAxis("7")}
	};
	
	private static IDictionary<GamepadBt, KeyCode> buttonsMappingAndroid = new Dictionary<GamepadBt, KeyCode>() {
		{GamepadBt.X, KeyCode.JoystickButton0},
		{GamepadBt.Circle, KeyCode.JoystickButton1},
		{GamepadBt.Square, KeyCode.JoystickButton2},
		{GamepadBt.Triangle, KeyCode.JoystickButton3},
		{GamepadBt.L1, KeyCode.JoystickButton4},
		{GamepadBt.R1, KeyCode.JoystickButton5},
		{GamepadBt.L3, KeyCode.JoystickButton8},
		{GamepadBt.R3, KeyCode.JoystickButton9},
		{GamepadBt.Options, KeyCode.JoystickButton10},
		{GamepadBt.Share, KeyCode.JoystickButton11}
	};
	private static IDictionary<GamepadBt, Tuple<String, Func<float, float>>> axisMappingAndroid = new Dictionary<GamepadBt, Tuple<String, Func<float, float>>>() {
		{GamepadBt.Lx, getStandardAxis("X")},
		{GamepadBt.Ly, getStandardAxis("Y")},
		{GamepadBt.Rx, getStandardAxis("3")},
		{GamepadBt.Ry, getStandardAxis("4")},
		{GamepadBt.L2, getStandardAxis("7")},
		{GamepadBt.R2, getStandardAxis("8")},
		{GamepadBt.CrossX, getStandardAxis("5")},
		{GamepadBt.CrossY, getStandardAxis("6")}
	};
	private static IDictionary<GamepadBt, KeyCode> buttonsMappingAndroidOld = new Dictionary<GamepadBt, KeyCode>() {
		{GamepadBt.X, KeyCode.JoystickButton1},
		{GamepadBt.Circle, KeyCode.JoystickButton13},
		{GamepadBt.Square, KeyCode.JoystickButton0},
		{GamepadBt.Triangle, KeyCode.JoystickButton2},
		{GamepadBt.L1, KeyCode.JoystickButton3},
		{GamepadBt.R1, KeyCode.JoystickButton14},
		{GamepadBt.L3, KeyCode.JoystickButton11},
		{GamepadBt.R3, KeyCode.JoystickButton10},
		{GamepadBt.Options, KeyCode.JoystickButton7},
		{GamepadBt.Share, KeyCode.JoystickButton6}
	};
	private static IDictionary<GamepadBt, Tuple<String, Func<float, float>>> axisMappingAndroidOld = new Dictionary<GamepadBt, Tuple<String, Func<float, float>>>() {
		{GamepadBt.Lx, getStandardAxis("X")},
		{GamepadBt.Ly, getStandardAxis("Y")},
		{GamepadBt.Rx, getStandardAxis("14")},
		{GamepadBt.Ry, getStandardAxis("15")},
		{GamepadBt.L2, getAxis0Centered("3")},
		{GamepadBt.R2, getAxis0Centered("4")},
		{GamepadBt.CrossX, getStandardAxis("5")},
		{GamepadBt.CrossY, getStandardAxis("6")}
	};
	
	private static Tuple<String, Func<float, float>> getStandardAxis(String axisName) {
		return new Tuple<String, Func<float, float>>(axisName, val => val);
	}
	
	private static Tuple<String, Func<float, float>> getAxis0Centered(String axisName) {
		return new Tuple<String, Func<float, float>>(axisName, val => (val + 1) / 2);
	}
	
	#if WEBGL && ! UNITY_EDITOR
		[DllImport("__Internal")]
		private static extern String getBrowserName();
		
		[DllImport("__Internal")]
		private static extern String getPlatform();
	
	#else
		private static String getBrowserName() {return null;}
		private static String getPlatform() {return null;}
	#endif
	
	public IDictionary<GamepadBt, KeyCode> buttonsMapping;
	public IDictionary<GamepadBt, Tuple<String, Func<float, float>>> axisMapping;
	public IDictionary<String, GamepadBt> actions = new Dictionary<String, GamepadBt>();
	
	private Action<String> logFunc;
	
	public GamepadInput() : this(msg => {}) {}
	public GamepadInput(Action<String> logFunc) {
		this.logFunc = logFunc;
		UpdateMapping();
	}
	
	private void UpdateMapping() {
		switch (Application.platform) {
			case RuntimePlatform.Android:
				String so = SystemInfo.operatingSystem;
				String apiLevel = so.Substring(so.IndexOf("API-", 0, so.Length) + 4, 2);
				this.logFunc("Android api: " + apiLevel);
				
				if (String.Compare(apiLevel, "26") < 0) { //Controller mapping in android depends on the OS version
					this.buttonsMapping = buttonsMappingAndroidOld;
					this.axisMapping = axisMappingAndroidOld;
				}
				
				else {
					this.buttonsMapping = buttonsMappingAndroid;
					this.axisMapping = axisMappingAndroid;
				}
				break;
			
			case RuntimePlatform.WebGLPlayer:
				String browserName = getBrowserName();
				this.logFunc("Browser name: " + browserName);
				
				if (browserName == "Chrome") {
					this.buttonsMapping = buttonsMappingChrome;
					this.axisMapping = axisMappingChrome;
				}
				else if (browserName == "Firefox") {
					String platform = getPlatform();
					this.logFunc("Platform:  " + platform);
					
					if (platform == "Linux") { //Controller mapping in firefox depends on the OS
						this.buttonsMapping = buttonsMappingFirefoxLinux;
						this.axisMapping = axisMappingFirefoxLinux;
					}
					else if (platform == "Win32") {
						this.buttonsMapping = buttonsMappingFirefoxWindows;
						this.axisMapping = axisMappingFirefoxWindows;
					}
				}
				break;
			
			case RuntimePlatform.LinuxPlayer:
			case RuntimePlatform.LinuxEditor:
				this.buttonsMapping = buttonsMappingLinux;
				this.axisMapping = axisMappingLinux;
				break;
			
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				String joystickName = Input.GetJoystickNames()[0];
				if (joystickName.Contains("Xbox")) { //XBox controller mapping in windows is the same as firefox in windows
					this.buttonsMapping = buttonsMappingFirefoxWindows;
					this.axisMapping = axisMappingFirefoxWindows;
				}
				else if (joystickName.Contains("Dualshock3")) { //Dualshock3 controller mapping in windows is the same as chrome
					this.buttonsMapping = buttonsMappingChrome;
					this.axisMapping = axisMappingChrome;
				}
				else {
					this.buttonsMapping = buttonsMappingWindows;
					this.axisMapping = axisMappingWindows;
				}
				break;
		}
	}
	
	public static bool IsActive() {
		return Input.GetJoystickNames().Where(x => ! x.StartsWith("uinput") && x.Length != 0).Any();
	}
	
	public void AddAction(String actionName, GamepadBt bt) {
		this.actions[actionName] = bt;
	}
	
	public bool GetButton(GamepadBt buttonName) {
		return Input.GetKey(this.buttonsMapping[buttonName]);
	}
	
	public float GetAxis(GamepadBt axisName) {
		Tuple<String, Func<float, float>> axis = this.axisMapping[axisName];
		return axis.Item2(Input.GetAxis(axis.Item1));
	}
	
	public bool GetButtonAction(String actionName) {
		return GetButton(this.actions[actionName]);
	}
	
	public float GetAxisAction(String actionName) {
		return GetAxis(this.actions[actionName]);
	}
}