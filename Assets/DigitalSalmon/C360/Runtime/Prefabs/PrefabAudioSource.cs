using System;
using System.Collections;
using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Examples/Prefab Audio Source")]
	public class PrefabAudioSource : InteractableBehaviour {
		[Flags]
		public enum StateActions {
			None          = 0,
			Pause         = 1,
			Stop          = 2,
			Play          = 4,
			RewindAndPlay = Stop | Play,
		}
		
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float fadeDuration;
		
		[SerializeField]
		protected bool loop;

		[SerializeField]
		protected StateActions onVisible;

		[SerializeField]
		protected StateActions onHidden;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private AudioSource    audioSource;
		private Sequence volumeSequence;
		
		//-----------------------------------------------------------------------------------------
		// Private Properties:
		//-----------------------------------------------------------------------------------------

		private PrefabElement Element { get; set; }

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected override void Awake() {
			base.Awake();
			volumeSequence= new Sequence(this);
			audioSource = GetComponent<AudioSource>();
		}

		protected void Start() { audioSource.loop = loop; }

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

			if (actions.HasFlag(StateActions.Pause)) audioSource.Pause();
			if (actions.HasFlag(StateActions.Stop)) {
				volumeSequence.Cancel();
				volumeSequence.Coroutine(FadeVolume(0, () => audioSource.Stop()));
				
			}
			if (actions.HasFlag(StateActions.Play)) {
				audioSource.Play();

				volumeSequence.Cancel();
				volumeSequence.Coroutine(FadeVolume(1));
			}
		}

		private IEnumerator FadeVolume(float targetVolume, Action onComplete =null) {
			
			if (fadeDuration <= 0) {
				audioSource.volume = targetVolume;
				onComplete?.Invoke();
				yield break;
			}
			
			float initialVolume = audioSource.volume;
			float alpha = 0;
			while (true) {
				alpha += UnityEngine.Time.deltaTime / fadeDuration;
				if (alpha > 1) alpha = 1;
				audioSource.volume = Mathf.Lerp(initialVolume, targetVolume, Mathf.Pow(alpha,2));
				if (alpha == 1) {
					onComplete?.Invoke();
					break;
				}
				yield return null;
			}
		}

	}
}