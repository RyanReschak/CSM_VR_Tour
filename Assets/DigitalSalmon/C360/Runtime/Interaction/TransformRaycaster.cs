using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Input/Transform Raycaster")]
	public class TransformRaycaster : UIInputRaycaster {
		protected override Methods Method => Methods.Physics;
		protected override Ray GetRay() => new Ray(transform.position, transform.forward);
	}
}