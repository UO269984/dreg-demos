using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
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
		this.xRotation += Input.GetAxis("Mouse X") * mult;
		this.yRotation += Input.GetAxis("Mouse Y") * mult;
		this.yRotation = Mathf.Clamp(this.yRotation, -80f, 20f);
		
		transform.localRotation = Quaternion.Euler(0, this.xRotation, this.yRotation);
	}
}