using System.Collections;
using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Input/Mouse Input Raycaster")]
	public class MouseInputRaycaster : UIInputRaycaster {
		[Header("Mouse Raycaster")]
		[SerializeField]
		protected bool clickToSubmit;
		
		protected override Vector2 GetInputPosition() => Input.mousePosition;

		protected void Update() {
			if (Input.GetMouseButtonDown(0) && clickToSubmit) {
				if (CurrentInteractable != null) {
					CurrentInteractable.Submit();
				}
			}
		}

	}
}