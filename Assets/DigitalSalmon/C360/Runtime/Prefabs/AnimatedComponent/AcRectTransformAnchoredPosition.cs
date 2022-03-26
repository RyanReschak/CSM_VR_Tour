using DigitalSalmon.Extensions;
using UnityEngine;

namespace DigitalSalmon.C360.AnimatedComponents {
	[AddComponentMenu("Complete 360 Tour/Animated Components/Rect Transform Anchored Position")]
	public class AcRectTransformAnchoredPosition : BaseBehaviour, IAnimatedComponent {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float positionA;

		[SerializeField]
		protected float positionB;

		[SerializeField]
		protected Axis2D axis;

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IAnimatedComponent.SetDelta(float alpha) {
			switch (axis) {
				case Axis2D.X:
					rectTransform.anchoredPosition = rectTransform.anchoredPosition.WithX(Mathf.Lerp(positionA, positionB, alpha));
					break;
				case Axis2D.Y:
					rectTransform.anchoredPosition = rectTransform.anchoredPosition.WithY(Mathf.Lerp(positionA, positionB, alpha));
					break;
			}
		}
	}
}