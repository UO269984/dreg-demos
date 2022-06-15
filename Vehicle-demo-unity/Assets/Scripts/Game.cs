using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	
	public GameObject pauseUI;
	
	private bool paused = false;
	
	public void Start() {
		Cursor.lockState = CursorLockMode.Locked;
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
		
		foreach (Vehicle vehicle in GetAllVehicles())
			vehicle.enabled = ! this.paused;
	}
	
	public void RestartGame() {
		foreach (Vehicle vehicle in GetAllVehicles())
			vehicle.Reset();
	}
	
	public void Exit() {
		Application.Quit();
	}
	
	private Vehicle[] GetAllVehicles() {
		return (Vehicle[]) GameObject.FindObjectsOfType(typeof(Vehicle));
	}
}