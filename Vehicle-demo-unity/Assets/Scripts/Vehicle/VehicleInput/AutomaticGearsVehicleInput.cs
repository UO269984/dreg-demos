using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticGearsVehicleInput : AutomaticVehicleInput {
	
	public float minRpm;
	public float maxRpm;
	
	public override void Reset() {
		base.Reset();
		this.controls.Gear = this.vehicle.Config.Power.NeutralIndex + 1;
	}
	
	protected override void UpdateGears() {
		if (! this.changingGear) {
			float rpm = this.vehicle.Props.EngineRpm;
			PowerConfig power = this.vehicle.Config.Power;
			
			if (rpm > this.maxRpm && this.controls.Gear < power.GearsCount - 1)
				ChangeGear(1);
			
			else if (rpm < this.minRpm && this.controls.Gear > power.NeutralIndex + 1)
				ChangeGear(-1);
		}
	}
}