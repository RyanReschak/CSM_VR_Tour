using DigitalSalmon.Extensions;
using UnityEngine;

namespace DigitalSalmon.C360.AnimatedComponents {
	[AddComponentMenu("Complete 360 Tour/Animated Components/Rect Transform Size")]
	public class AcRectTransformSize : BaseBehaviour, IAnimatedComponent {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float sizeA;

		[SerializeField]
		protected float sizeB;

		[SerializeField]
		protected Axis2D axis;

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IAnimatedComponent.SetDelta(float alpha) {
			switch (axis) {
				case Axis2D.X:
					rectTransform.sizeDelta = rectTransform.sizeDelta.WithX(Mathf.Lerp(sizeA, sizeB, alpha));
					break;
				case Axis2D.Y:
					rectTransform.sizeDelta = rectTransform.sizeDelta.WithY(Mathf.Lerp(sizeA, sizeB, alpha));
					break;
			}
		}
	}
}