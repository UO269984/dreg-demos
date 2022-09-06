using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedConfigLoader : ConfigLoader {
	
	public int neutralIndex = 0;
	
	[TextArea(30, 1)]
	public String serializedConfig;
	
	public override void LoadConfig(ConfigManager configManager) {
		configManager.LoadSerializedConfig(this.serializedConfig);
		configManager.Config.Power.NeutralIndex = this.neutralIndex;
	}
}