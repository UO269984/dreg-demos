using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRefsConfigLoader : ConfigLoader {
	
	public Transform vehicleCenter;
	public Transform frontShaft;
	public Transform rearShaft;
	public Renderer referenceWheel;
	
	public override void LoadConfig(ConfigManager configManager) {
		VehicleConfig config = configManager.Config;
		
		config.FrontShaft = this.frontShaft.position - this.vehicleCenter.position;
		config.RearShaft = this.rearShaft.position - this.vehicleCenter.position;
		config.Wheels.Diameter = this.referenceWheel.bounds.size.y;
	}
}