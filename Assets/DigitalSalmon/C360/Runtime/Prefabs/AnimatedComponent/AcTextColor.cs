using DigitalSalmon.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DigitalSalmon.C360.AnimatedComponents {
	[AddComponentMenu("Complete 360 Tour/Animated Components/Text Colour")]
	public class AcTextColor : BaseBehaviour, IAnimatedComponent {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected Color colorA;

		[SerializeField]
		protected Color colorB;

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
			if (Text != null) Text.color = Color.Lerp(colorA.WithAlpha(Text.color.a), colorB.WithAlpha(Text.color.a), alpha);
			if (TmpText != null) TmpText.color = Color.Lerp(colorA.WithAlpha(TmpText.color.a), colorB.WithAlpha(TmpText.color.a), alpha);
		}
	}
}