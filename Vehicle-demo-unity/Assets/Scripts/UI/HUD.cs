using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
	
	public Color throttleColor;
	public Color brakeColor;
	public Color clutchColor;
	
	private Text gearText;
	private Text speedText;
	private Text rpmText;
	
	private ProgressBar throttleBar;
	private ProgressBar brakeBar;
	private ProgressBar clutchBar;
	
	public void Start() {
		this.gearText = this.transform.Find("GearBG/Gear").GetComponent<Text>();
		this.speedText = this.transform.Find("Speed").GetComponent<Text>();
		this.rpmText = this.transform.Find("rpm").GetComponent<Text>();
		
		this.throttleBar = this.transform.Find("PedalsPanel/ThrottleStatus").GetComponent<ProgressBar>();
		this.brakeBar = this.transform.Find("PedalsPanel/BrakeStatus").GetComponent<ProgressBar>();
		this.clutchBar = this.transform.Find("PedalsPanel/ClutchStatus").GetComponent<ProgressBar>();
		this.throttleBar.SetColor(this.throttleColor);
		this.brakeBar.SetColor(this.brakeColor);
		this.clutchBar.SetColor(this.clutchColor);
	}
	
	public void UpdateHUD(Vehicle vehicle, VehicleControls controls) {
		int neutralIndex = vehicle.Config.Power.NeutralIndex;
		
		this.gearText.text = controls.Gear != neutralIndex ? (controls.Gear - neutralIndex).ToString() : "N";
		this.speedText.text = (vehicle.Props.Speed * 3.6).ToString("0.0") + " Km/h";
		this.rpmText.text = vehicle.Props.EngineRpm.ToString("0") + " rpm";
		
		this.throttleBar.SetProgress(controls.Throttle);
		this.brakeBar.SetProgress(controls.Brake);
		this.clutchBar.SetProgress(controls.Clutch);
	}
	
	public Color Transparent(Color color) {
		return new Color(color.r, color.g, color.b, 0.62f);
	}
}