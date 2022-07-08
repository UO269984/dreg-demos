using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

public class InputLogger {
	
	private IntPtr vehiclePtr;
	private IntPtr inputLogger = IntPtr.Zero;
	
	public InputLogger(Vehicle vehicle) {
		this.vehiclePtr = vehicle.VehiclePtr;
	}
	
	public bool IsActive() {
		return this.inputLogger != IntPtr.Zero;
	}
	
	private void CheckActive() {
		if (! IsActive())
			throw new InvalidOperationException("Logging not started");
	}
	
	public void StartLogging() {
		this.inputLogger = Dreg.createInputLogger(this.vehiclePtr);
	}
	
	public void LogInput(float delta) {
		CheckActive();
		Dreg.logInput(this.inputLogger, delta);
	}
	
	public void SaveInput(String filename) {
		CheckActive();
		
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