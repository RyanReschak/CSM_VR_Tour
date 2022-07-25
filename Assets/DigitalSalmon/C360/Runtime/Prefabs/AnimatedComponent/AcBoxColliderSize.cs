using DigitalSalmon.Extensions;
using UnityEngine;

namespace DigitalSalmon.C360.AnimatedComponents {
	[AddComponentMenu("Complete 360 Tour/Animated Components/Box Collider Size")]
	public class AcBoxColliderSize : BaseBehaviour, IAnimatedComponent {
		//-----------------------------------------------------------------------------------------
		// Inspector variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float sizeA;

		[SerializeField]
		protected float sizeB;

		[SerializeField]
		protected Axis3D axis;

		//-----------------------------------------------------------------------------------------
		// Backing Fields:
		//-----------------------------------------------------------------------------------------

		private BoxCollider _boxCollider;

		//-----------------------------------------------------------------------------------------
		// Private Properties:
		//-----------------------------------------------------------------------------------------

		private BoxCollider BoxCollider => _boxCollider == null ? (_boxCollider = GetComponent<BoxCollider>()) : _boxCollider;

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IAnimatedComponent.SetDelta(float alpha) {
			switch (axis) {
				case Axis3D.X:
					BoxCollider.size = BoxCollider.size.WithX(Mathf.Lerp(sizeA, sizeB, alpha));
					break;
				case Axis3D.Y:
					BoxCollider.size = BoxCollider.size.WithY(Mathf.Lerp(sizeA, sizeB, alpha));
					break;
				case Axis3D.Z:
					BoxCollider.size = BoxCollider.size.WithZ(Mathf.Lerp(sizeA, sizeB, alpha));
					break;
			}
		}
	}
}