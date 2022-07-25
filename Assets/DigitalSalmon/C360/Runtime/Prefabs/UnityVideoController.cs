using DigitalSalmon.Extensions;
using UnityEngine;
using UnityEngine.Video;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Core/Unity Video Controller")]
	public class UnityVideoController : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Events:
		//-----------------------------------------------------------------------------------------

		public event EventHandler VideoPlaybackStarted;
		public event EventHandler VideoLooped;
		public event EventHandler TargetTextureChanged;

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Resolution Control")]
		[Tooltip("If true the video will be played at it's native resolution, ignoring the resolution override")]
		[SerializeField]
		protected bool nativeResolution;

		[Tooltip("If native resolution is false, this resolution is used for playback (Useful for performance improvement)")]
		[SerializeField]
		protected Vector2Int resolutionOverride = new Vector2Int(1024, 512);

		[Header("Video Settings")]
		[SerializeField]
		protected bool loop;

		[Header("Audio Settings")]
		[SerializeField]
		protected VideoAudioOutputMode audioOutputMode;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private VideoPlayer videoPlayer;

		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public RenderTexture TargetTexture { get; private set; }
		public bool IsPlaying => videoPlayer.isPlaying;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() {
			videoPlayer = this.GetOrAddComponent<VideoPlayer>();

			videoPlayer.skipOnDrop = true;
			videoPlayer.audioOutputMode = audioOutputMode;
			videoPlayer.isLooping = loop;

			if (audioOutputMode == VideoAudioOutputMode.AudioSource) {
				AudioSource audioSource = this.GetOrAddComponent<AudioSource>();
				videoPlayer.SetTargetAudioSource(0, audioSource);
			}
		}

		protected void OnEnable() {
			videoPlayer.prepareCompleted += VideoPlayer_PrepareCompleted;
			videoPlayer.loopPointReached += VideoPlayer_LoopPointReached;
		}

		protected void OnDisable() {
			videoPlayer.prepareCompleted -= VideoPlayer_PrepareCompleted;
			videoPlayer.loopPointReached -= VideoPlayer_LoopPointReached;
		}

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		private void VideoPlayer_PrepareCompleted(VideoPlayer source) {
			videoPlayer.Play();
			VideoPlaybackStarted.InvokeSafe();
		}

		private void VideoPlayer_LoopPointReached(VideoPlayer source) { VideoLooped.InvokeSafe(); }

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public float AspectRatio() {
			if (TargetTexture == null) return 2;
			return (float) TargetTexture.width / TargetTexture.height;
		}

		public void SetLooping(bool looping) {
			loop = looping;
			videoPlayer.isLooping = looping;
		}

		public void Stop() { videoPlayer.Stop(); }

		public void Pause() { videoPlayer.Pause(); }

		public void PlayClip(VideoClip videoClip) {
			videoPlayer.Stop();

			if (videoClip != null) {
				CreateVideoTextureIfRequired(new Vector2Int((int) videoClip.width, (int) videoClip.height));

				videoPlayer.source = VideoSource.VideoClip;
				videoPlayer.clip = videoClip;
			}
			else {
				SetTargetTexture(null);
				return;
			}

			videoPlayer.Prepare();
		}

		public void PlayURL(string url) {
			videoPlayer.Stop();

			if (!string.IsNullOrEmpty(url)) {
				CreateVideoTextureIfRequired(new Vector2Int(resolutionOverride.x, resolutionOverride.y));

				videoPlayer.source = VideoSource.Url;
				videoPlayer.url = url;
			}
			else {
				SetTargetTexture(null);
				return;
			}

			videoPlayer.Prepare();
		}
		

		// Configure out surface to display the appropriate texture.
		//		Surface.SetYFlip(false);
		//		Surface.SetTexture(videoTexture);
		//		Surface.SetStereoscopic(data.IsStereo);
		//IO.FormatPath(Path.Combine(Application.streamingAssetsPath, IO.StreamingAssetsPathFromAbsPath(data.AbsPath)))

//		Time.ResetTimer();
//		if (videoPlayer.isLooping && videoPlayer.isPlaying) Time.StartTimer();

		public void CreateVideoTextureIfRequired(Vector2Int size) {
			if (TargetTexture != null && TargetTexture.Size() == size) return;

			// If we already have a videoTexture, Destroy it after clearing the content.
			if (TargetTexture != null) {
				TargetTexture.DiscardContents();
				Destroy(TargetTexture);
			}

			TargetTexture = new RenderTexture(size.x, size.y, 0, RenderTextureFormat.ARGB32);
			TargetTexture.autoGenerateMips = false;
			SetTargetTexture(TargetTexture);
		}

		private void SetTargetTexture(RenderTexture renderTexture) {
			videoPlayer.targetTexture = renderTexture;
			TargetTextureChanged.InvokeSafe();
		}
	}
}