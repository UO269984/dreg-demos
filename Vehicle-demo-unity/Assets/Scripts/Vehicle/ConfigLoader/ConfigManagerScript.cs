using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManagerScript : MonoBehaviour {
	
	public bool saveInitData = false;
	public ConfigLoader[] configLoaders;
	
	public ConfigManager ConfigManager {get; private set;}
	
	public ConfigManager LoadConfig() {
		foreach (ConfigLoader loader in this.configLoaders)
			loader.LoadConfig(this.ConfigManager);
		
		this.ConfigManager.Update();
		return this.ConfigManager;
	}
	
	public void Awake() {
		Graph.SetSaveInitData(this.saveInitData);
		this.ConfigManager = new ConfigManager(true);
	}
	
	public void OnDestroy() {
		this.ConfigManager.Delete();
	}
}