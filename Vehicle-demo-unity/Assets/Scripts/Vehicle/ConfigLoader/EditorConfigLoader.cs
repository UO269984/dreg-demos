using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorConfigLoader : ConfigLoader {
	
	public Transform frontShaft;
	public Transform rearShaft;
	public float maxSteeringAngle = 20;
	public float mass = 500;
	
	public float torqueToRpmAccel = 10;
	public float driveRatio = 1;
	public int neutralIndex = 0;
	public float[] gearRatios;
	
	public override void LoadConfig(VehicleConfig config) {
		config.Power.TorqueToRpmAccel = this.torqueToRpmAccel;
		config.Power.GearRatios = this.gearRatios;
		config.Power.NeutralIndex = this.neutralIndex;
		config.Power.DriveRatio = this.driveRatio;
		
		config.Wheels.Diameter = 0.5f;
		
		config.FrontShaft = this.frontShaft.position - transform.position;
		config.RearShaft = this.rearShaft.position - transform.position;
		config.MaxSteeringAngle = this.maxSteeringAngle;
		config.Mass = this.mass;
		
		InitGraphs(config);
	}
	
	private void InitGraphs(VehicleConfig config) {
		config.Power.ThrottleCurve.LoadLinear(new Vector2[] {
			new Vector2(0, 0.02f),
			new Vector2(0.05f, 0.02f),
			new Vector2(1, 1)});
		
		config.Power.EngineCurve.LoadLinear(new Vector2[] {
			new Vector2(0, 50),
			new Vector2(1000, 500)});
		
		config.Power.LooseEngineRpmCurve.LoadLinear(new Vector2[] {
			new Vector2(0, 0),
			new Vector2(1, 5000)});
		
		config.Power.EngineBrakeCurve.LoadLinear(new Vector2[] {
			new Vector2(0, 0),
			new Vector2(250, -40),
			new Vector2(5000, -100)});
		
		config.Power.ClutchCurve.LoadLinear(new Vector2[] {
			new Vector2(0, 600),
			new Vector2(1, 0)});
	}
}