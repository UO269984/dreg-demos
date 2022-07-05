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
	
	public void Start() {
		this.vehiclePtr = GetComponent<Vehicle>().GetVehiclePtr();
	}
	
	public void Update() {
		if (InputManager.input.GetAxisAction("InputLogging") < 0) {
			this.loggingEnabled = ! this.loggingEnabled;
			
			if (this.loggingEnabled)
				StartLoggingInput();
			
			else {
				StopLoggingInput(
					Application.platform == RuntimePlatform.WebGLPlayer ?
					this.inputLoggingFileWeb : this.inputLoggingFile);
			}
		}
		
		if (this.inputLogger != IntPtr.Zero)
			Dreg.logInput(this.inputLogger, Time.deltaTime);
	}
	
	public void StartLoggingInput() {
		this.recordingText.SetActive(true);
		this.inputLogger = Dreg.createInputLogger(this.vehiclePtr);
	}
	
	public void StopLoggingInput(String filename) {
		this.recordingText.SetActive(false);
		
		IntPtr filenameCharPtr = Marshal.AllocHGlobal((filename.Length + 1) * Size.Char);
		int i = 0;
		foreach (char c in filename)
			Marshal.WriteByte(filenameCharPtr, i++, (Byte) c);
		
		Marshal.WriteByte(filenameCharPtr, i, 0);
		
		Dreg.saveInputLogger(this.inputLogger, filenameCharPtr);
		Marshal.FreeHGlobal(filenameCharPtr);
		this.inputLogger = IntPtr.Zero;
	}
}