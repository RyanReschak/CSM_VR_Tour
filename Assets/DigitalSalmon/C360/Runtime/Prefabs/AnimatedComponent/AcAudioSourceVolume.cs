using UnityEngine;

namespace DigitalSalmon.C360.AnimatedComponents {
	[AddComponentMenu("Complete 360 Tour/Animated Components/Audio Source Volume")]
	public class AcAudioSourceVolume : BaseBehaviour, IAnimatedComponent {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float volumeA;

		[SerializeField]
		protected float volumeB;

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IAnimatedComponent.SetDelta(float alpha) { audio.volume = Mathf.Lerp(volumeA, volumeB, alpha); }
	}
}