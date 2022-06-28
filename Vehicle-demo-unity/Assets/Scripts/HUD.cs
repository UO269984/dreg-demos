using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
	
	public Transform hudUI;
	public Color throttleColor;
	public Color brakeColor;
	public Color clutchColor;
	
	[HideInInspector]
	public int neutralIndex;
	
	private Text gearText;
	private Text speedText;
	private Text rpmText;
	
	public void Start() {
		this.gearText = this.hudUI.Find("GearBG/Gear").GetComponent<Text>();
		this.speedText = this.hudUI.Find("Speed").GetComponent<Text>();
		this.rpmText = this.hudUI.Find("rpm").GetComponent<Text>();
	}
	
	public void UpdateHUD(VehicleControls controls, VehicleProps props) {
		this.gearText.text = controls.gear != this.neutralIndex ? (controls.gear - this.neutralIndex).ToString() : "N";
		this.speedText.text = (props.speed * 3.6).ToString("0.0") + " Km/h";
		this.rpmText.text = props.engineRpm.ToString("0") + " rpm";
	}
	
	public Color Transparent(Color color) {
		return new Color(color.r, color.g, color.b, 0.62f);
	}
}