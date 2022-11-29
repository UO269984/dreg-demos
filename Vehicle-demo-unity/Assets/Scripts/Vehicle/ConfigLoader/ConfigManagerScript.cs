using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManagerScript : MonoBehaviour {
	
	public bool saveInitData = false;
	public ConfigLoader[] configLoaders;
	
	private bool loaded = false;
	private ConfigManager configManager;
	
	public ConfigManager ConfigManager {get {
			
			if (! this.loaded) {
				foreach (ConfigLoader loader in this.configLoaders)
					loader.LoadConfig(this.configManager);
				
				this.configManager.Update();
				this.loaded = true;
			}
			return this.configManager;
		}}
	
	public void Awake() {
		Graph.SetSaveInitData(this.saveInitData);
		this.configManager = new ConfigManager(true);
	}
	
	public void OnDestroy() {
		this.configManager.Delete();
	}
}