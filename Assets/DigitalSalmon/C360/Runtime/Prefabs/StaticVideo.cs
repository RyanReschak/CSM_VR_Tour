using DigitalSalmon.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Examples/Static Video")]
	public class StaticVideo : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Video Settings")]
		[SerializeField]
		protected VideoClip videoClip;

		[SerializeField]
		protected string videoURL;

		[Header("Transform Settings")]
		[SerializeField]
		protected float width;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private UnityVideoController videoController;
		private RawImage rawImage;
		private Canvas canvas;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() { LocateComponents(); }

		protected void OnEnable() { videoController.TargetTextureChanged += VideoController_TargetTextureChanged; }

		protected void Start() {

			InitialiseTransform();

			if (videoClip != null) videoController.PlayClip(videoClip);
			else videoController.PlayURL(videoURL);
		}

		protected void OnDisable() { videoController.TargetTextureChanged -= VideoController_TargetTextureChanged; }

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		private void VideoController_TargetTextureChanged() { rawImage.texture = videoController.TargetTexture; }

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void LocateComponents() {
			rawImage = GetComponentInChildren<RawImage>();
			canvas = GetComponentInChildren<Canvas>();
			videoController = this.GetOrAddComponent<UnityVideoController>();
		}

		private void InitialiseTransform() {
			if (rawImage != null) {
				((RectTransform) canvas.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
				((RectTransform) canvas.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width * (1f / videoController.AspectRatio()));
			}
		}
	}
}