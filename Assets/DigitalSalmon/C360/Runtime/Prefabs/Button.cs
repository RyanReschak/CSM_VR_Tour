using System.Collections;
using System.Collections.Generic;
using DigitalSalmon.C360;
using TMPro;
using UnityEngine;

public class Button : AnimatedBehaviour {
	[SerializeField]
	protected TMP_Text label;

	protected void Start() {
		EnableInteraction();
	}
	
	public void SetLabelText(string text) {
		label.text = text;
	}
}