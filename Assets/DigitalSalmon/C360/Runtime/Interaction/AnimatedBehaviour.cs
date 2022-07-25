using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityTime = UnityEngine.Time;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Interaction/Animated Behaviour")]
	public abstract class AnimatedBehaviour : InteractableBehaviour {
		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		[FormerlySerializedAs("componentAnimator")]
		[Header("Animated Behaviour")]
		[SerializeField]
		protected ComponentAnimator hoverAnimator;

		[SerializeField]
		[Range(1,3)]
		protected float animationSpeed = 1;
		
		[Header("Developer")]
		[SerializeField]
		protected bool visualiseDelta;

		[SerializeField]
		[Range(0, 1f)]
		protected float delta;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private float hoveredAlpha;
		private bool  triggerLocked;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected virtual void OnValidate() {
			if (!Application.isPlaying && visualiseDelta) {
				OnHoveredDeltaUpdate(delta);
			}
		}

		protected override void OnDisable() {
			base.OnDisable();
			StopAllCoroutines();
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected override void OnHoveredChanged(bool hovered) {
			base.OnHoveredChanged(hovered);
			triggerLocked = false;
			if (!hovered) sequence.Coroutine(ReduceInteractionTime());
		}

		protected virtual void OnHoveredDeltaUpdate(float delta) {
			if (hoverAnimator != null) hoverAnimator.SetDelta(delta);
		}

		protected override void OnInteractionTimeChanged(float t, float totalTime) {
			base.OnInteractionTimeChanged(t, totalTime);
			delta = t / totalTime;
			delta *= animationSpeed;
			delta = Mathf.Clamp01(delta);
			OnHoveredDeltaUpdate(Easing.QuadEaseInOut(delta));
		}

		protected virtual void OnHoverAnimationComplete() { Submit(); }

		private IEnumerator ReduceInteractionTime() {
			while (true) {
				delta -= UnityTime.deltaTime * 2;
				if (delta < 0) delta = 0;

				OnHoveredDeltaUpdate(delta);

				if (delta == 0) break;
				yield return null;
			}
		}
	}
}