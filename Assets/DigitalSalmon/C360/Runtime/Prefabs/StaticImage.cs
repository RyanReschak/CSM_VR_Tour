using UnityEngine;
using UnityEngine.UI;

namespace DigitalSalmon.C360 {
	[ExecuteInEditMode]
	[AddComponentMenu("Complete 360 Tour/Examples/Static Image")]
	public class StaticImage : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected Texture2D image;

		[SerializeField]
		protected float width;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private Canvas canvas;
		private RawImage rawImage;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() { LocateComponents(); }

		protected void Start() { Initialise(); }

		protected void Update() {
			if (Application.isPlaying) return;
			LocateComponents();
			Initialise();
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void LocateComponents() {
			rawImage = GetComponentInChildren<RawImage>();
			canvas = GetComponentInChildren<Canvas>();
		}

		private void Initialise() {
			if (rawImage != null) {
				((RectTransform) canvas.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
				((RectTransform) canvas.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width * ((float) image.height / image.width));

				rawImage.texture = image;
			}
		}
	}
}