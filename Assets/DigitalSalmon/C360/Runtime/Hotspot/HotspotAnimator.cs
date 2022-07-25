using DigitalSalmon.DarkestFayte;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalSalmon.C360 {
	public class HotspotAnimator : ComponentAnimator {
		[SerializeField]
		protected HotspotMeshEffect hotspotMesheffect;
		
		public override void SetDelta(float delta) {
			if (hotspotMesheffect != null) hotspotMesheffect.SetFill(delta);
		}
	}
	
	
}