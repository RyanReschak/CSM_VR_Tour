namespace DigitalSalmon.C360 {
	public class MediaClock {
		public double StartTimeMs { get; private set; }
		public double TimeMs => GetTime();

		private bool Looping { get; }
		private double LoopTimeMs { get; }

		private double Limit { get; set; }

		private double ElapsedTimeMs => ((UnityEngine.Time.time*1000) - StartTimeMs);

		public MediaClock() {
			StartTimeMs = UnityEngine.Time.time * 1000;
			Looping = false;
			Limit = 0;
		}

		protected MediaClock(double loopTimeMs) : this() {
			LoopTimeMs = loopTimeMs;
			Looping = true;
		}

		private double GetTime() {
			if (Limit != 0 && ElapsedTimeMs > Limit) return Limit;
			
			while (Looping && ElapsedTimeMs > LoopTimeMs) {
				StartTimeMs += LoopTimeMs;
			}

			return ElapsedTimeMs;
		}

		public static MediaClock Looped(double loopTime) => new MediaClock(loopTime);
		public static MediaClock Limited(double limit) => new MediaClock() { Limit = limit };
	}
}