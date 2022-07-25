using DigitalSalmon.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalSalmon.C360.AnimatedComponents {
	[AddComponentMenu("Complete 360 Tour/Animated Components/Graphic Colour")]
	public class AcGraphicColor : BaseBehaviour, IAnimatedComponent {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		protected Color colorA;

		[SerializeField]
		protected Color colorB;

		//-----------------------------------------------------------------------------------------
		// Backing Fields:
		//-----------------------------------------------------------------------------------------

		private Graphic _graphic;

		//-----------------------------------------------------------------------------------------
		// Private Properties:
		//-----------------------------------------------------------------------------------------

		private Graphic Graphic => _graphic == null ? (_graphic = GetComponent<Image>()) : _graphic;

		//-----------------------------------------------------------------------------------------
		// Interface Methods:
		//-----------------------------------------------------------------------------------------

		void IAnimatedComponent.SetDelta(float alpha) { Graphic.color = Color.Lerp(colorA.WithAlpha(Graphic.color.a), colorB.WithAlpha(Graphic.color.a), alpha); }
	}
}