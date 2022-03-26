using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Core/Hotspot Reactor")]
	public class HotspotReactor : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private readonly HashSet<Hotspot> activeHotspots = new HashSet<Hotspot>();

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void OnEnable() { Complete360Tour.MediaSwitch += C360_MediaSwitch; }

		protected void OnDisable() { Complete360Tour.MediaSwitch -= C360_MediaSwitch; }

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		protected void C360_MediaSwitch(TransitionState state, Node node) {
			if (state == TransitionState.BeforeSwitch) DestroyHotspots();
			if (state == TransitionState.Switch && node != null) ShowHotspots(node);
		}
		

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void DestroyHotspots() {
			foreach (Hotspot hotspot in activeHotspots) {
				if (hotspot != null) hotspot.HideAndDestroy();
			}
			activeHotspots.Clear();
		}

		private void ShowHotspots( Node node) {
			if (node.HotspotElements == null || !node.HotspotElements.Any()) return;

			foreach (HotspotElement hotspotElement in node.HotspotElements) {
				if (hotspotElement.Hotspot == null) {
					Debug.LogWarning($"Cannot create Hotspot GameObject for {hotspotElement.Name}. Have you assigned a default Hotspot in your Tour? Are you overriding the Hotspot without assigning an override GameObject?");
					continue;
				}
				Hotspot hotspot = Instantiate(hotspotElement.Hotspot, transform).GetComponent<Hotspot>();

				hotspot.Construct(node, hotspotElement);
				activeHotspots.Add(hotspot);
			}
		}
	}
}