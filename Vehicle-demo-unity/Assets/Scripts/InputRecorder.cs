using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

public class InputRecorder : MonoBehaviour {
	
	public GameObject recordingText;
	
	private IntPtr vehiclePtr;
	private IntPtr inputLogger = IntPtr.Zero;
	
	public String inputLoggingFile = "InputLog.csv";
	public String inputLoggingFileWeb = "InputLog.csv";
	
	private bool loggingEnabled = false;
	private bool loggingBtEnabled = false;
	
	public void Start() {
		this.vehiclePtr = GetComponent<Vehicle>().GetVehiclePtr();
	}
	
	public void Update() {
		if (InputManager.input.GetAxisAction("InputLogging") < 0) {
			if (! this.loggingBtEnabled) {
				this.loggingEnabled = ! this.loggingEnabled;
				
				if (this.loggingEnabled)
					StartLoggingInput();
				
				else {
					StopLoggingInput(
						Application.platform == RuntimePlatform.WebGLPlayer ?
						this.inputLoggingFileWeb : this.inputLoggingFile);
				}
			}
			this.loggingBtEnabled = true;
		}
		else
			this.loggingBtEnabled = false;
		
		if (this.inputLogger != IntPtr.Zero)
			DrivingEngine.logInput(this.inputLogger, Time.deltaTime);
	}
	
	public void StartLoggingInput() {
		this.recordingText.SetActive(true);
		this.inputLogger = DrivingEngine.createInputLogger(this.vehiclePtr);
	}
	
	public void StopLoggingInput(String filename) {
		this.recordingText.SetActive(false);
		
		IntPtr filenameCharPtr = Marshal.AllocHGlobal((filename.Length + 1) * Size.Char);
		int i = 0;
		foreach (char c in filename)
			Marshal.WriteByte(filenameCharPtr, i++, (Byte) c);
		
		Marshal.WriteByte(filenameCharPtr, i, 0);
		
		DrivingEngine.saveInputLogger(this.inputLogger, filenameCharPtr);
		Marshal.FreeHGlobal(filenameCharPtr);
		this.inputLogger = IntPtr.Zero;
	}
}