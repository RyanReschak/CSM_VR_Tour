using System;
using DigitalSalmon.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Examples/Popup Video")]
	public class PopupVideo : AnimatedBehaviour {
		[Flags]
		public enum StateActions {
			None  = 0,
			Pause = 1,
			Stop  = 2,
			Play  = 4,
			RewindAndPlay = Stop | Play,
		}

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Video Settings")]
		[SerializeField]
		protected VideoClip videoClip;

		[SerializeField]
		protected string videoURL;

		[Header("Popup Settings")]
		[SerializeField]
		protected StateActions onVisible = StateActions.Stop;

		[SerializeField]
		protected StateActions onHidden = StateActions.Stop;

		[SerializeField]
		protected StateActions onHover = StateActions.Play;

		[SerializeField]
		protected StateActions onUnhover = StateActions.Pause;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private UnityVideoController videoController;
		private RawImage             rawImage;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected override void Awake() {
			base.Awake();
			LocateComponents();
		}

		protected void Start() {
			EnableInteraction();
		}

		protected override void OnEnable() {
			base.OnEnable();
			videoController.TargetTextureChanged += VideoController_TargetTextureChanged;
		}

		protected override void OnDisable() {
			base.OnDisable();
			videoController.TargetTextureChanged -= VideoController_TargetTextureChanged;
		}

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		private void VideoController_TargetTextureChanged() { rawImage.texture = videoController.TargetTexture; }

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public void Play() {
			if (videoClip != null) videoController.PlayClip(videoClip);
			else videoController.PlayURL(videoURL);
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected override void OnHoveredChanged(bool hovered) {
			base.OnHoveredChanged(hovered);
			if (hovered) {
				ApplyStateActions(onHover);
			}
			else {
				ApplyStateActions(onUnhover);
			}
		}

		protected override void OnVisiblityChanged(bool visible) {
			base.OnVisiblityChanged(visible);
			if (visible) {
				ApplyStateActions(onVisible);
			}
			else {
				ApplyStateActions(onHidden);
			}
		}

		private void ApplyStateActions(StateActions actions) {
			if (actions == StateActions.None) return;

			if (actions.HasFlag(StateActions.Pause)) videoController.Pause();
			if (actions.HasFlag(StateActions.Stop)) videoController.Stop();
			if (actions.HasFlag(StateActions.Play)) Play();
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void LocateComponents() {
			rawImage = GetComponentInChildren<RawImage>();
			videoController = this.GetOrAddComponent<UnityVideoController>();
		}
	}
}