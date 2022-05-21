using System.Collections;
using UnityEngine;
using UnityEngine.XR;

//Made by Ryan Reschak

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour XR based Raycaster")]
	public class InputRaycaster_XR : UIInputRaycaster {
		[Header("XR Controller Raycaster")]
		[SerializeField]
		protected bool clickToSubmit;
		UnityEngine.XR.InputDevice Hand_device;

		protected override Vector2 GetInputPosition() => Input.mousePosition;

		protected void Update() {

			bool triggerValue;
			if (Hand_device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && clickToSubmit)
			{
				CurrentInteractable.Submit();
			}
		}

	}
}