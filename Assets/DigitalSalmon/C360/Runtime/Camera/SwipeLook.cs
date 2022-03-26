using UnityEngine;
using UnityTime = UnityEngine.Time;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Camera/Swipe Look")]
	public class SwipeLook : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float spinSpeed = 10f;

		[SerializeField]
		protected float smoothing = 0.25f;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private float activeVelocityY;
		private float activeVelocityX;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Update() {
			activeVelocityX = Mathf.Lerp(activeVelocityX, 0, 1f / smoothing * UnityTime.deltaTime);
			activeVelocityY = Mathf.Lerp(activeVelocityY, 0, 1f / smoothing * UnityTime.deltaTime);

			if (Input.touchCount == 1) {
				activeVelocityX += -Input.touches[0].deltaPosition.y / Screen.height * spinSpeed;
				activeVelocityY += Input.touches[0].deltaPosition.x / Screen.width * spinSpeed;
			}

			transform.localEulerAngles += new Vector3(activeVelocityX, activeVelocityY, 0);
			float xAngle = transform.localEulerAngles.x;
			if (xAngle > 80 && xAngle < 180) xAngle = 80;
			if (xAngle > 180 && xAngle < 280) xAngle = 280;
			transform.localEulerAngles = new Vector3(xAngle, transform.localEulerAngles.y, 0);
		}
	}
}