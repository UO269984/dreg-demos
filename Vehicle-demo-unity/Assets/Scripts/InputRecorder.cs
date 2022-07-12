using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRecorder : MonoBehaviour {
	
	public GameObject recordingText;
	private InputLogger inputLogger;
	
	public String inputLoggingFile = "InputLog.csv";
	public String inputLoggingFileWeb = "InputLog.csv";
	
	private bool loggingEnabled = false;
	
	public void Start() {
		this.inputLogger = new InputLogger(GetComponent<BaseVehicle>().Vehicle);
	}
	
	public void Update() {
		if (InputManager.input.GetAxisAction("InputLogging") < 0) {
			this.loggingEnabled = ! this.loggingEnabled;
			
			if (this.loggingEnabled)
				StartLoggingInput();
			
			else
				StopLoggingInput();
		}
		
		if (this.inputLogger.IsActive())
			this.inputLogger.LogInput(Time.deltaTime);
	}
	
	public void StartLoggingInput() {
		this.recordingText.SetActive(true);
		this.inputLogger.StartLogging();
	}
	
	public void StopLoggingInput() {
		this.recordingText.SetActive(false);
		this.inputLogger.SaveInput(Application.platform == RuntimePlatform.WebGLPlayer ?
			this.inputLoggingFileWeb : this.inputLoggingFile);
	}
}