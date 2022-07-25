namespace DigitalSalmon.C360 {
	public partial class Complete360Tour {
		public class Warnings {
			public static string MISSING_NODEDATA_NAME(string name) => $"Could not find Node name:[{name}], ignoring 'GoToMedia' call.";
			public static string MISSING_NODEDATA_UID(uint uid) => $"Could not find Node uid:[{uid}], ignoring 'GoToMedia' call.";

			public static string MISSING_LOADER => "No default TourLoader assigned to Complete360Tour object. No tour will be loaded.";
			public static string AUTOPLAY_MISSINGTOUR => "Auto Begin Tour is set to true, but no Tour has been loaded yet (Does your TourLoader Auto Load?)";
			public static string GOTOMEDIA_MISSINGDATA => "GoToMedia called but requested nodeData is null.";
			public static string MISSING_TRANSITION => "Not Transition component added to Complete360Tour object. Make sure you add one! For example: Fade Transition";
			public static string MISSING_FIRSTMEDIA => "Could not load first media. Is your Tour loaded?";

			//Loaders
			public static string MISSING_TOURTEXTDATA =>
				"Attempting to Load Tour from TextAssetTourLoader without a valid TextAsset. Ensure a 'TourData' TextAsset has been assigned Complete360Tour object in your scene.";

			public static string TOURTEXTDATA_FAILEDLOAD => "Failed to load tour. TourData file could be damaged.";
		}
	}
}