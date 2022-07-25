using System.Diagnostics;

namespace DigitalSalmon.C360 {
	public class TimelineHelper {
		//-----------------------------------------------------------------------------------------
		// Public Events:
		//-----------------------------------------------------------------------------------------

		public event EventHandler ActiveChanged;
		public event EventHandler ThresholdCrossed;

		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public bool IsActive { get; private set; }

		public bool ThresholdHasCrossed { get; private set; }

		public bool Debug { get; set; }

		//-----------------------------------------------------------------------------------------
		// Private Properties:
		//-----------------------------------------------------------------------------------------

		protected ITimedElement TimedElement { get; private set; }

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public void Update(bool forceEvent = false) {
			bool shouldBeActive = GetShouldBeActive();
			if (Debug) UnityEngine.Debug.Log(shouldBeActive);
			bool changed = IsActive != shouldBeActive;
			IsActive = shouldBeActive;
			if (changed || forceEvent) {
				ActiveChanged?.Invoke();
			}
		}

		public void AssignElement(ITimedElement timedElement) {
			TimedElement = timedElement;
			Update(true);
		}

		private bool GetShouldBeActive() {
			if (TimedElement == null) {
				return true;
			}
			if (!TimedElement.UseTimeline) return true;

			if (Complete360Tour.ActiveNode is MediaNode timelineNode && timelineNode.DurationMs == 0) {
				return true;
			}

			double currentTime = Time.TimeMs;

			bool afterEntry = currentTime >= TimedElement.EntryTime;
			bool beforeExit = currentTime <= TimedElement.ExitTime;

			if (!afterEntry && ThresholdHasCrossed) {
				ThresholdHasCrossed = false;
			}

			if (afterEntry && !ThresholdHasCrossed) {
				ThresholdHasCrossed = true;
				ThresholdCrossed?.Invoke();
			}

			return afterEntry && beforeExit;
		}
	}
}