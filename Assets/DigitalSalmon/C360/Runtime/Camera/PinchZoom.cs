using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Input/Pinch Zoom")]
	public class PinchZoom : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float sensitivity = 1;

		[SerializeField]
		protected float minFov = 10f;

		[SerializeField]
		protected float maxFov = 110f;

		[SerializeField]
		protected float smoothing = 3f;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private Camera cam;

		private bool wasZoomingLastFrame; // Touch mode only
		private Vector2[] lastZoomPositions; // Touch mode only

		private float targetFov;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() {
			cam = GetComponent<Camera>();
			targetFov = cam.fieldOfView;
		}

		protected void Update() {
			if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer) {
				HandleTouch();
			}
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void HandleTouch() {
			switch (Input.touchCount) {
				case 2: // Zooming
					Vector2[] newPositions = {Input.GetTouch(0).position, Input.GetTouch(1).position};
					if (!wasZoomingLastFrame) {
						lastZoomPositions = newPositions;
						wasZoomingLastFrame = true;
					}
					else {
						// Zoom based on the distance between the new positions compared to the 
						// distance between the previous positions.
						float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
						float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
						float offset = newDistance - oldDistance;
						targetFov = Mathf.Clamp(cam.fieldOfView - offset * sensitivity, minFov, maxFov);

						lastZoomPositions = newPositions;
					}
					break;

				default:
					wasZoomingLastFrame = false;
					break;
			}

			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, 1f / smoothing);
		}
	}
}