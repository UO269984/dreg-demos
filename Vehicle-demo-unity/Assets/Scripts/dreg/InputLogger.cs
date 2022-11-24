using System;

using System.Runtime.InteropServices;

public class InputLogger {
	
	private IntPtr vehiclePtr;
	private IntPtr inputLogger = IntPtr.Zero;
	
	public InputLogger(Vehicle vehicle) {
		this.vehiclePtr = vehicle.Ptr;
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
		
		IntPtr filenameCharPtr = Marshal.StringToCoTaskMemAnsi(filename);
		Dreg.saveInputLogger(this.inputLogger, filenameCharPtr);
		Marshal.FreeCoTaskMem(filenameCharPtr);
		
		this.inputLogger = IntPtr.Zero;
	}
}