using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Core/Unity Video Reactor")]
	public class UnityVideoReactor : MediaReactor, ITimeController {
		[Serializable]
		public struct LoadOptions {
			[SerializeField]
			public Texture2D Texture;

			[SerializeField]
			public bool IsStereoscopic;

			[SerializeField]
			public MediaProjection Projection;
		}

		//-----------------------------------------------------------------------------------------
		// Serialized Fields:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected LoadOptions loadOptions;

		[SerializeField]
		protected MediaTransition mediaTransition;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private VideoPlayer videoPlayer;

		int ITimeController.Priority => 1;
		double ITimeController.Time => GetOrCreateVideoPlayer().time * 1000;

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected override void Awake() {
			base.Awake();
			videoPlayer = GetComponent<VideoPlayer>();
		}

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		protected override void C360_MediaSwitch(TransitionState state, Node node) {
			if (state == TransitionState.Switch) SwitchMedia(node);
		}

		protected void SwitchMedia(Node node) {
			Time.UnregisterController(this);

			// Locate a valid VideoPlayer component.
			videoPlayer = GetOrCreateVideoPlayer();
			videoPlayer.Stop();

			// Stop any running sequence co-routines.
			sequence.Cancel();

			if (!(node is VideoNode videoNode)) return;

			sequence.Coroutine(SwitchMedia(videoNode));
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private VideoPlayer GetOrCreateVideoPlayer() {
			if (videoPlayer != null) return videoPlayer;

			videoPlayer = gameObject.AddComponent<VideoPlayer>();
			
			return videoPlayer;
		}

		private void ShowLoading() {
			Surface.SetYFlip(false);
			Surface.SetTexture(loadOptions.Texture);
			Surface.SetStereoscopic(loadOptions.IsStereoscopic);
			Surface.SetProjection(loadOptions.Projection);
		}

		private void ShowMedia(VideoNode node) {
			Surface.SetYFlip(false);
			Surface.SetTexture(videoPlayer.texture);
			Surface.SetStereoscopic(node.IsStereoscopic);
			Surface.SetProjection(node.Projection);
		}

		private IEnumerator SwitchMedia(VideoNode node) {
			// Make sure systems know were loading.
			mediaTransition?.RegisterLoadObject(this);
			ShowLoading();

			// Set up the videoPlayer to work with this load system and API.
			videoPlayer.playOnAwake = false;
			videoPlayer.renderMode = VideoRenderMode.APIOnly;
			videoPlayer.waitForFirstFrame = false;
			videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;

			// Set/reset the player.
			videoPlayer.Stop();

			// If we have nothing to play, back out.
			if (node == null) yield break;
			
			
			List<IVideoLoadData> loadDatas = ListPool<IVideoLoadData>.New();
			node.GetLoadData(loadDatas);

			foreach (IVideoLoadData loadData in loadDatas) {
				if (!loadData.CanAttemptLoad()) continue;
				
				
				if (loadData is VideoClipVideoLoadData videoClipLoadData) {
#if !UNITY_WEBGL
					videoPlayer.clip = videoClipLoadData.VideoClip;
					break;
#else
					Debug.LogWarning("Unity WebGL does not support VideoClip playback. Use URL LoadData, VideoClip LoadData will be ignored.");
#endif
				}
			

				if (loadData is UrlVideoLoadData urlClipLoadData) {
					videoPlayer.url = urlClipLoadData.Url;
					break;
				}
			}
			
			// Set up looping.
			videoPlayer.isLooping = node.Loop;

			// Let the player prepare.
			videoPlayer.Prepare();
			while (!videoPlayer.isPrepared) yield return null;

			// Stop load blocking.
			mediaTransition?.UnregisterLoadObject(this);

			Time.RegisterController(this);

			// Play the newly prepared video.
			videoPlayer.Play();

			// Show the video media texture.
			ShowMedia(node);
		}
		
		
	}
}