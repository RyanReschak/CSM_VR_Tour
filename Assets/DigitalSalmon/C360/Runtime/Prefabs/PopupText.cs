using UnityEngine;
using TMPro;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Examples/Text Popup")]
	public class PopupText : AnimatedBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Text Settings")]
		[SerializeField]
		protected string title;

		[SerializeField]
		protected string subtitle;

		[SerializeField]
		[TextArea]
		protected string body;

		[Header("Reference")]
		[SerializeField]
		protected TMP_Text titleText;

		[SerializeField]
		protected TMP_Text subtitleText;

		[SerializeField]
		protected TMP_Text bodyText;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected override void OnValidate() {
			base.OnValidate();
			Initialise();
		}

		protected void Start() {
			Initialise(); 
			EnableInteraction();
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void Initialise() {
			if (titleText != null) titleText.text = title;
			if (subtitleText != null) subtitleText.text = subtitle;
			if (bodyText != null) bodyText.text = body;
		}
	}
}