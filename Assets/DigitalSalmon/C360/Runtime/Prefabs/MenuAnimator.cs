using UnityEngine;

namespace DigitalSalmon.C360 {
	public class MenuAnimator : ComponentAnimator {
		[SerializeField]
		protected CanvasGroup canvasGroup;

		public override void SetDelta(float delta) {
			if (canvasGroup != null) canvasGroup.alpha = delta;
		}
	}
}