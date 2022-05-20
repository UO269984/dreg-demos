using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCamera : MonoBehaviour {
	public float lookSpeed = 200f;
	
	private float xRotation;
	private float yRotation;
	
	public void Start() {
		this.xRotation = transform.localRotation.x;
		this.yRotation = transform.localRotation.y;
		
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	public void Update() {
		float mult = this.lookSpeed * Time.deltaTime;
		this.xRotation += InputManager.input.GetAxisAction("Camera-X") * mult;
		this.yRotation -= InputManager.input.GetAxisAction("Camera-Y") * mult;
		this.yRotation = Mathf.Clamp(this.yRotation, -80f, 25f);
		
		transform.localRotation = Quaternion.Euler(0, this.xRotation, this.yRotation);
	}
}