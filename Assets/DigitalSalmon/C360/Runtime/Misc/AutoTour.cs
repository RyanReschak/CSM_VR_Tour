using System.Collections;
using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Examples/Auto Tour")]
	public class AutoTour : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Events:
		//-----------------------------------------------------------------------------------------

		public event EventHandler Complete;

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Tooltip("The names, in order, of the nodes this AutoTour should traverse.")]
		[SerializeField]
		protected Node[] nodes;

		[Tooltip("If true the AutoTour will begin as soon as you press Play.")]
		[SerializeField]
		protected bool autoStart;

		[Tooltip("If true the AutoTour will loop back to the first node when it reaches the end")]
		[SerializeField]
		protected bool loop;

		[Tooltip("The length of time to spend in each node")]
		[SerializeField]
		protected float nodeDuration;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected IEnumerator Start() {
			// Delay a frame to let C360 initialise.
			yield return null;
			if (autoStart) BeginAutoTour();
		}

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public void BeginAutoTour() { StartCoroutine(AutoTourCoroutine()); }

		public void StopAutoTour() { StopAllCoroutines(); }

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private IEnumerator AutoTourCoroutine() {
			WaitForSeconds wait = new WaitForSeconds(nodeDuration);
			int index = 0;
			while (true) {
				
				Node nextNode = nodes[index];
				Complete360Tour.GoToMedia(nextNode);

				index = GetNextIndex(index);
				if (index == 0) {
					if (Complete != null) Complete();
					if (!loop) break;
				}

				yield return wait;
			}
		}

		private int GetNextIndex(int index) {
			if (index >= nodes.Length - 1) return 0;
			return index + 1;
		}
	}
}