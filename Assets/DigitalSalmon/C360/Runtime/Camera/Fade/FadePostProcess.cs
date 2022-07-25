using System;
using System.Collections;
using UnityEngine;
using UnityTime = UnityEngine.Time;

namespace DigitalSalmon.C360 {
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	[AddComponentMenu("Complete 360 Tour/Camera/Fade Post Process")]
	public class FadePostProcess : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Constants:
		//-----------------------------------------------------------------------------------------

		private const string ALPHA = "_Alpha";
		private const string DELTA = "_Delta";

		//-----------------------------------------------------------------------------------------
		// Events:
		//-----------------------------------------------------------------------------------------

		public event EventHandler FadedUp;
		public event EventHandler FadedDown;

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected Shader effectShader;

		[SerializeField]
		protected float effectDuration = 1;

		[SerializeField]
		protected AnimationCurve effectEasing;

		[Header("Developer")]
		[SerializeField]
		protected bool preview;

		[SerializeField]
		protected float previewAlphaPercentage;

		//-----------------------------------------------------------------------------------------
		// Backings Fields:
		//-----------------------------------------------------------------------------------------

		private float alpha;
		private Material _currentEffectMaterial;

		//-----------------------------------------------------------------------------------------
		// Private Properties:
		//-----------------------------------------------------------------------------------------

		private Material CurrentEffectMaterial {
			get {
				if (_currentEffectMaterial == null && effectShader != null) {
					_currentEffectMaterial = new Material(effectShader);
				}
				return _currentEffectMaterial;
			}
		}

		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public bool IsFadedDown => alpha == 1;
		public bool IsFadedUp => alpha == 0;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() {
			if (!Application.isPlaying) return;

			StopAllCoroutines();
			alpha = 1;
			SetDeltaFromAlpha();
		}

		protected void OnValidate() {
			if (!Application.isPlaying) {
				if (preview) {
					alpha = previewAlphaPercentage / 100f;
					SetDeltaFromAlpha();
				}
			}
		}

		protected void OnRenderImage(RenderTexture source, RenderTexture destination) {
			if (CurrentEffectMaterial == null || !Application.isPlaying && !preview || GetEffectAlpha() == 0) {
				Graphics.Blit(source, destination);
				return;
			}

			Graphics.Blit(source, destination, CurrentEffectMaterial);
		}

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public void Cancel() {
			StopAllCoroutines();
			alpha = 0;
			SetDeltaFromAlpha();
		}

		/// <summary>
		/// Fade down then back up again immediately.
		/// </summary>
		public void Dip() {
			StopAllCoroutines();
			StartCoroutine(FadeUpCoroutine(() => FadeUp()));
		}

		/// <summary>
		/// Fades 'Down' (Displays the fade effect).
		/// </summary>
		public void FadeDown(bool instant = false, Action onComplete = null) {
			StopAllCoroutines();
			if (instant) {
				alpha = 1;
				SetDeltaFromAlpha();
				if (onComplete != null) onComplete();
				return;
			}
			StartCoroutine(FadeDownCoroutine(onComplete));
		}

		/// <summary>
		/// Fades 'Up' (Hides the fade effect).
		/// </summary>
		public void FadeUp(bool instant = false, Action onComplete = null) {
			StopAllCoroutines();
			if (instant) {
				alpha = 0;
				SetDeltaFromAlpha();
				if (onComplete != null) onComplete();
				return;
			}
			StartCoroutine(FadeUpCoroutine(onComplete));
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected void InvokeFadedUp() {
			if (FadedUp != null) FadedUp.Invoke();
		}

		protected void InvokeFadedDown() {
			if (FadedDown != null) FadedDown.Invoke();
		}

		protected float GetEffectAlpha() {
			if (CurrentEffectMaterial == null) return 0;
			return CurrentEffectMaterial.GetFloat(ALPHA);
		}

		protected void SetDeltaFromAlpha() {
			if (CurrentEffectMaterial == null) return;
			CurrentEffectMaterial.SetFloat(ALPHA, alpha);
			float delta = effectEasing == null ? alpha : effectEasing.Evaluate(alpha);
			CurrentEffectMaterial.SetFloat(DELTA, delta);
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private IEnumerator FadeDownCoroutine(Action onComplete) {
			alpha = GetEffectAlpha();
			while (true) {
				alpha += UnityTime.unscaledDeltaTime / effectDuration;

				if (alpha >= 1) {
					alpha = 1;
				}
				SetDeltaFromAlpha();

				if (alpha == 1) {
					yield return new WaitForEndOfFrame();

					InvokeFadedDown();
					if (onComplete != null) onComplete.Invoke();
					break;
				}

				yield return null;
			}
		}

		private IEnumerator FadeUpCoroutine(Action onComplete) {
			alpha = GetEffectAlpha();
			while (true) {
				alpha -= UnityTime.unscaledDeltaTime / effectDuration;

				if (alpha <= 0) {
					alpha = 0;
				}

				SetDeltaFromAlpha();

				if (alpha == 0) {
					yield return new WaitForEndOfFrame();

					InvokeFadedUp();
					if (onComplete != null) onComplete.Invoke();
					break;
				}

				yield return null;
			}
		}
	}
}