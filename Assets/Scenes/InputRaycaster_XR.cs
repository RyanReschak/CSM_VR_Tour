using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
//Made by Ryan Reschak

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour XR based Raycaster")]
	public class InputRaycaster_XR : UIInputRaycaster {
		[Header("XR Controller Raycaster")]
		[SerializeField]
		protected bool clickToSubmit;
		public UnityEngine.XR.InputDevice Hand_device;
		public UnityEngine.XR.Interaction.Toolkit.XRRayInteractor xrRay;
		//public Vector2 pos = new Vector2(0.5f, 0.5f);

		protected override Vector2 GetInputPosition()
        {
			//The index of the last hit point
			//Only is set when a hit occured... 0 if no hit occured
			RaycastHit hit;
			if(xrRay.TryGetCurrent3DRaycastHit(out hit))
            {
				//pos = hit.point;
				return hit.point;
            }
			Vector3[] points = null;
			int numPoints;
			xrRay.GetLinePoints(ref points, out numPoints);
			//pos = points[numPoints-1];
			return points[numPoints - 1];
			//return xrRay.m_RaycastHits[xrRay.m_RaycastHitEndpointIndex].point;
			//xrRay.TryGetCurrent3DRaycastHit();

		}

		//protected override Ray GetRay() => filler;//*** something here from XRRAyInteractor??


		protected void Update() {

			bool triggerValue;
			if (Hand_device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && clickToSubmit)
			{
				CurrentInteractable.Submit();
			}
		}

	}
}