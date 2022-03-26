using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalSalmon.C360 {
	public class ButtonAnimator : ComponentAnimator {
		[Header("Components")]
		[SerializeField]
		protected Image fillImage;

		[SerializeField]
		protected TMP_Text labelText;

		[Header("Animation")]
		[SerializeField]
		protected Color labelStart;

		[SerializeField]
		protected Color labelEnd;

		public override void SetDelta(float delta) {
			if (fillImage != null) {
				fillImage.color = new Color(1-delta, 0, 0, 1);
			}

			if (labelText != null) {
				labelText.color = Color.Lerp(labelStart, labelEnd, delta);
			}
		}
	}
}