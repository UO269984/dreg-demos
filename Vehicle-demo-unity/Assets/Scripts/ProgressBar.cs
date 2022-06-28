using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
	
	private RectTransform progressBarBox;
	private RectTransform progressBox;
	private Image progressImg;
	
	public void Start() {
		this.progressBarBox = GetComponent<RectTransform>();
		
		GameObject progressObj = transform.GetChild(0).gameObject;
		this.progressBox = progressObj.GetComponent<RectTransform>();
		this.progressImg = progressObj.GetComponent<Image>();
	}
	
	public void SetColor(Color color) {
		this.progressImg.color = color;
	}
	
	public void SetProgress(float percent) {
		this.progressBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
			this.progressBarBox.rect.height * percent);
	}
}