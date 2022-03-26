using System.Collections;
using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Transition/Fade Transition")]
	public class FadeTransition : MediaTransition {
		//-----------------------------------------------------------------------------------------
		// Serialized Fields:
		//-----------------------------------------------------------------------------------------

		[Header("Assignment")]
		[SerializeField]
		protected FadePostProcess fade;

		[SerializeField]
		protected float loadDurationTimeout = 5;

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public override void Interrupt() {
			IsTransitioning = false;
			fade.Cancel();
			sequence.Cancel();
		}

		public override void StartTransition(Node node) {
			IsTransitioning = true;
			sequence.Coroutine(TransitionCoroutine(node));
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------
		
		private IEnumerator TransitionCoroutine(Node node) {
			IsTransitioning = true;
			
			InvokeMediaSwitch(TransitionState.BeforeSwitch, node);
			fade.FadeDown();
			while (!fade.IsFadedDown) yield return null;
			InvokeMediaSwitch(TransitionState.Switch, node);
			while (IsLoading) yield return null;
			yield return null;
			fade.FadeUp();
			while (!fade.IsFadedUp) yield return null;
			InvokeMediaSwitch(TransitionState.AfterSwitch, node);
			IsTransitioning = false;
		}
	}
}