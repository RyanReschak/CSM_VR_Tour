using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Camera/Gyro Look")]
	public class GyroLook : MonoBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------
		//"Sets this transform rotation to the rotation of the device gyroscope if gyro is supported.")]

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private float yRotation;
		private float xRotation;

		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public bool Supported => SystemInfo.supportsGyroscope;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Start() {
			if (Supported) {
				Input.gyro.enabled = true;
				Input.gyro.updateInterval = 1F / 60F;
			}
			else {
				Debug.LogWarning($"{nameof(GyroLook)} is enabled, but SystemInfo.supportsGyroscope is false");
			}
		}

		protected void Update() {
			if (Supported) {
				/*
				float xDiff = Input.gyro.rotationRateUnbiased.x;
				float yDiff = Input.gyro.rotationRateUnbiased.y;
	
				float angle = Vector3.Angle(Vector3.forward, Input.gyro.attitude * Vector3.forward);
				yDiff += Input.gyro.rotationRateUnbiased.z * (-(angle - 90) / 90);
	
				if (xDiff > 180) xDiff -= 360;
				else if (xDiff < -180) xDiff += 360;
	
				if (yDiff > 180) yDiff -= 360;
				else if (yDiff < -180) yDiff += 360;
	
				xRotation -= xDiff;
				yRotation -= yDiff;
				transform.localEulerAngles = new Vector3(xRotation, yRotation, 0);*/

				//transform.localRotation = GyroToUnity(Input.gyro.attitude);

				transform.rotation = Input.gyro.attitude;
				transform.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
				transform.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
			}
		}




		private static Quaternion GyroToUnity(Quaternion q) { return new Quaternion(q.x, q.y, -q.z, -q.w); }
	}
}