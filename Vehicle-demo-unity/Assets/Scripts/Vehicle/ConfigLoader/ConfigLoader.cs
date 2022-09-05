using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConfigLoader : MonoBehaviour {
	
	public bool saveInitData = false;
	
	private bool loaded = false;
	private ConfigManager configManager;
	
	public ConfigManager ConfigManager {get {
			
			if (! this.loaded) {
				LoadConfig(this.configManager);
				this.configManager.Update();
				this.loaded = true;
			}
			return this.configManager;
		}}
	
	public void Awake() {
		Graph.SetGraphSaveInitData(this.saveInitData);
		this.configManager = new ConfigManager(true);
	}
	
	public void OnDestroy() {
		this.configManager.Delete();
	}
	
	public abstract void LoadConfig(ConfigManager configManager);
}