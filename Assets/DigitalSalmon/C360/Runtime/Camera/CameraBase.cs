using DigitalSalmon;
using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Camera/Camera Base")]
	public class CameraBase : BaseBehaviour {
		public void SetYaw(float yaw) { transform.localEulerAngles = new Vector3(0, yaw, 0); }

		public float GetCameraYaw() => transform.GetChild(0).localEulerAngles.y;
	}
}