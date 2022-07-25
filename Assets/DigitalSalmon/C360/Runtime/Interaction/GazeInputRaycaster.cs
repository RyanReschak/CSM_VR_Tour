using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Input/Gaze Input Raycaster")]
	public class GazeInputRaycaster : UIInputRaycaster {
		protected override Vector2 GetInputPosition() => camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
	}
}