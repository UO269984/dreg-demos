using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchButton : Button {
	public bool Pressed { get {return this.IsPressed();}}
	
	public Action MouseDownCallback { get; set; }
	public Action MouseUpCallback { get; set; }
	
	public TouchButton() {
		this.MouseDownCallback = () => {};
		this.MouseUpCallback = () => {};
	}
	
	public override void OnPointerDown(PointerEventData e) {
		base.OnPointerDown(e);
		MouseDownCallback();
	}
	
	public override void OnPointerUp(PointerEventData e) {
		base.OnPointerUp(e);
		MouseUpCallback();
	}
}