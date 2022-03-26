using UnityEngine;

namespace DigitalSalmon.C360 {
	public class KeepScreenAwake : BaseBehaviour {
		protected void Awake() { Screen.sleepTimeout = SleepTimeout.NeverSleep; }
	}
}