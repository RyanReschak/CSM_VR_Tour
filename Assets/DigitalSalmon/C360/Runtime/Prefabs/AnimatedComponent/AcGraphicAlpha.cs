using DigitalSalmon.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalSalmon.C360.AnimatedComponents {
	[AddComponentMenu("Complete 360 Tour/Animated Components/Graphic Alpha")]
	public class AcGraphicAlpha : BaseBehaviour, IAnimatedComponent {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected float alphaA;

		[SerializeField]
		protected float alphaB;

		//-----------------------------------------------------------------------------------------
		// Backing Fields:
		//-----------------------------------------------------------------------------------------

		private Graphic _graphic;

		//-----------------------------------------------------------------------------------------
		// Private Properties:
		//-----------------------------------------------------------------------------------------

		private Graphic Graphic => _graphic == null ? (_graphic = GetComponent<Graphic>()) : _graphic;

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IAnimatedComponent.SetDelta(float alpha) { Graphic.color = Color.Lerp(Graphic.color.WithAlpha(alphaA), Graphic.color.WithAlpha(alphaB), alpha); }
	}
}