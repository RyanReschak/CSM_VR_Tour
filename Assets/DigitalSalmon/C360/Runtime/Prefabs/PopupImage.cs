using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Examples/Popup Image")]
	public class PopupImage : AnimatedBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Image Popup")]
		[SerializeField]
		protected Texture2D image;

		[SerializeField]
		protected Sprite icon;

		[SerializeField]
		protected string title;

		[SerializeField]
		[TextArea]
		protected string subtitle;

		[Header("References")]
		[SerializeField]
		protected RawImage rawImage;

		[SerializeField]
		protected Image iconImage;

		[SerializeField]
		protected TMP_Text titleText;

		[SerializeField]
		protected TMP_Text subtitleText;

		[SerializeField]
		protected RectTransform overlayContainer;

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
			if (rawImage != null) rawImage.texture = image;
			if (iconImage != null) iconImage.sprite = icon;
			if (titleText != null) titleText.text = title;
			if (subtitleText != null) subtitleText.text = subtitle;

			if (overlayContainer != null) overlayContainer.gameObject.SetActive(!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(subtitle));
		}
	}
}