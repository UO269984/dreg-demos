using System;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeInput : IInput {
	private float maxAngle;
	private Vector3 refVector;
	private Vector3 planeNormal;
	
	public GyroscopeInput(float maxAngle, float refAngle = 35) {
		this.maxAngle = maxAngle;
		this.refVector = Quaternion.Euler(-refAngle, 0, 0) * Vector3.back;
		this.planeNormal = Quaternion.Euler(-90 - refAngle, 0, 0) * Vector3.back;
		
		if (IsActive())
			Input.gyro.enabled = true;
	}
	
	public static bool IsActive() {
		return SystemInfo.supportsGyroscope;
	}
	
	public bool GetButtonAction(String actionName) {
		return false;
	}
	
	public float GetAxisAction(String actionName) {
		Vector3 onPlane = Vector3.ProjectOnPlane(Input.gyro.gravity, this.planeNormal);
		float angle = Vector3.Angle(onPlane, this.refVector);
		angle = Math.Min(angle / maxAngle, 1);
		
		return onPlane.x - this.refVector.x > 0 ? angle : -angle;
	}
}