using System.Collections;
using System.Collections.Generic;
using DigitalSalmon.DarkestFayte;
using UnityEngine;

namespace DigitalSalmon.C360 {
    public class Crosshair : MonoBehaviour {
		[Header("Crosshair")]
		[SerializeField]
		protected HotspotMeshEffect meshEffect;

		[SerializeField]
		protected UIInputRaycaster raycaster;

		[Header("Settings")]
		[SerializeField]
		protected bool showFill;

		protected void OnEnable() {
			raycaster.InteractionAlphaChanged += Raycaster_InteractionAlphaChanged;
		}

		protected void OnDisable() {
			raycaster.InteractionAlphaChanged -= Raycaster_InteractionAlphaChanged;
		}
		
		private void Raycaster_InteractionAlphaChanged(float value) {
			if (showFill) meshEffect.SetFill(value);
			else meshEffect.SetFill(1);
		}
	}
}
