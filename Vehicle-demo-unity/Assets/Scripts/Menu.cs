using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {
	
	public void OnEnable() {
		EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
	}
}