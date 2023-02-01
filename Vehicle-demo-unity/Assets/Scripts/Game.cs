using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Runtime.InteropServices;

public class Game : MonoBehaviour {
	
	#if WEBGL && ! UNITY_EDITOR
		[DllImport("__Internal")]
		private static extern void getClipboard(String gameObject, String method);
	#else
		private static void getClipboard(String gameObject, String method) {}
	#endif
	
	public GameObject pauseUI;
	public GameObject[] hudObjects;
	public GameObject touchControlsUI;
	public TouchButtonManager touchButtonManager;
	public BaseVehicle playerVehicle;
	
	private SerializedConfigLoader configLoader;
	private bool paused = false;
	
	public void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		
		if (InputManager.inputType == InputType.Touch) {
			this.touchControlsUI.SetActive(true);
			this.touchButtonManager.LoadButtons();
			
			//Move HUD elements to make room for touch buttons
			foreach (GameObject hudObject in this.hudObjects)
				hudObject.transform.localPosition += new Vector3(0, 100, 0);
		}
		
		this.configLoader = this.playerVehicle.configManagerScript.GetComponent<SerializedConfigLoader>();
	}
	
	public void Update() {
		if (InputManager.input.GetButtonAction("Pause"))
			TogglePause();
	}
	
	public void TogglePause() {
		this.paused = ! this.paused;
		
		Time.timeScale = this.paused ? 0 : 1;
		Cursor.lockState = this.paused ? CursorLockMode.None : CursorLockMode.Locked;
		this.pauseUI.SetActive(this.paused);
		
		foreach (BaseVehicle vehicle in GetAllVehicles())
			vehicle.enabled = ! this.paused;
	}
	
	public void RestartGame() {
		foreach (BaseVehicle vehicle in GetAllVehicles())
			vehicle.Reset();
	}
	
	public void ChangeConfig() {
		if (Application.platform == RuntimePlatform.WebGLPlayer)
			getClipboard("Game", "SetPlayerVehicleConfig");
		
		else
			SetPlayerVehicleConfig(GUIUtility.systemCopyBuffer);
	}
	
	private void SetPlayerVehicleConfig(String text) {
		this.configLoader.serializedConfig = text.Replace("\r", "");
		this.playerVehicle.LoadConfig();
	}
	
	public void Exit() {
		Application.Quit();
	}
	
	private BaseVehicle[] GetAllVehicles() {
		return GameObject.FindObjectsOfType<BaseVehicle>();
	}
}