using UnityEngine;

namespace DigitalSalmon.C360.AnimatedComponents {
	[AddComponentMenu("Complete 360 Tour/Animated Components/Sphere Collider Radius")]
	public class AcSphereColliderRadius : BaseBehaviour, IAnimatedComponent {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float radiusA;

		[SerializeField]
		protected float radiusB;

		//-----------------------------------------------------------------------------------------
		// Backing Fields:
		//-----------------------------------------------------------------------------------------

		private SphereCollider _sphereCollider;

		//-----------------------------------------------------------------------------------------
		// Private Properties:
		//-----------------------------------------------------------------------------------------

		private SphereCollider SphereCollider => _sphereCollider == null ? (_sphereCollider = GetComponent<SphereCollider>()) : _sphereCollider;

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IAnimatedComponent.SetDelta(float alpha) { SphereCollider.radius = Mathf.Lerp(radiusA, radiusB, alpha); }
	}
}