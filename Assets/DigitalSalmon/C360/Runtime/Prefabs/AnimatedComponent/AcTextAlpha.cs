using DigitalSalmon.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DigitalSalmon.C360.AnimatedComponents {
	[AddComponentMenu("Complete 360 Tour/Animated Components/Text Alpha")]
	public class AcTextAlpha : BaseBehaviour, IAnimatedComponent {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float alphaA;

		[SerializeField]
		protected float alphaB;

		//-----------------------------------------------------------------------------------------
		// Backing Fields:
		//-----------------------------------------------------------------------------------------

		private Text _text;
		private TMP_Text _tmpText;

		//-----------------------------------------------------------------------------------------
		// Private Properties:
		//-----------------------------------------------------------------------------------------

		private Text Text => _text == null ? (_text = GetComponent<Text>()) : _text;
		private TMP_Text TmpText => _tmpText == null ? (_tmpText = GetComponent<TMP_Text>()) : _tmpText;

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IAnimatedComponent.SetDelta(float alpha) {
			if (Text != null) Text.color = Color.Lerp(Text.color.WithAlpha(alphaA), Text.color.WithAlpha(alphaB), alpha);
			if (TmpText != null) TmpText.color = Color.Lerp(TmpText.color.WithAlpha(alphaA), TmpText.color.WithAlpha(alphaB), alpha);
		}
	}
}