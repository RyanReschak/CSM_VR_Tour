using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Core/Entry Yaw Reactor")]
	public class EntryYawReactor : MediaReactor {
		//-----------------------------------------------------------------------------------------
		// Type Definitions:
		//-----------------------------------------------------------------------------------------

		public enum EntryYawModes {
			Absolute,
			Dynamic
		}

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[SerializeField]
		[Tooltip("Absolute | The world is aligned to the entry yaw defined by the node editor.\n Dynamic | The camera will always be looking at the entry yaw line when the node is entered")]
		protected EntryYawModes entryYawMode;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private CameraBase cameraBase;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected override void Awake() {
			base.Awake();
			cameraBase = FindObjectOfType<CameraBase>();
		}

		protected override void C360_MediaSwitch(TransitionState state, Node node) {
			if (!(node is MediaNode mediaNode)) return;
			if (state == TransitionState.Switch) UpdateEntryYaw(mediaNode.EntryYaw);
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void UpdateEntryYaw(float entryYaw) {
			if (cameraBase == null) return;
			switch (entryYawMode) {
				case EntryYawModes.Absolute:
					cameraBase.SetYaw(entryYaw * 360);
					break;
				case EntryYawModes.Dynamic:
					float cameraYaw = cameraBase.GetCameraYaw();
					cameraBase.SetYaw(entryYaw * 360 - cameraYaw);
					break;
			}
		}
	}
}