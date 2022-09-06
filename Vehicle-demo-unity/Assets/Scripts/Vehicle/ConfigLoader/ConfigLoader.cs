using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConfigLoader : MonoBehaviour {
	public abstract void LoadConfig(ConfigManager configManager);
}