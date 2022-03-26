using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Complete 360 Tour")]
	public partial class Complete360Tour : MonoBehaviour {
		//-----------------------------------------------------------------------------------------
		// Events:
		//-----------------------------------------------------------------------------------------

		public static event EventHandler<TransitionState, Node> MediaSwitch;

		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Tour Settings")]
		[Header("Assignment")]
		[SerializeField]
		protected Tour tour;

		[SerializeField]
		[Tooltip("The first node to load in your tour.")]
		protected Node entryNode;

		[SerializeField]
		protected MediaTransition transition;

		[Header("Loading")]
		[SerializeField]
		[Tooltip("Should the tour begin as soon as the scene loads.")]
		protected bool autoBeginTour = true;
		
		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		/// <summary>
		/// The currently loaded Tour.
		/// </summary>
		public static Tour ActiveTour { get; private set; }

		/// <summary>
		/// The upcoming NodeData (Valid once a transition has started).
		/// </summary>
		public static Node NextNode { get; private set; }

		/// <summary>
		// The previous NodeData (Valid once a transition has started).
		/// </summary>
		public static Node PreviousNode { get; private set; }

		/// <summary>
		/// The current NodeData we are viewing.
		/// </summary>
		public static Node ActiveNode { get; private set; }
		
		protected static Complete360Tour Instance { get; private set; }

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() {
			ActiveTour = tour;
			Instance = this;
		}

		protected void OnEnable() {
			transition.MediaSwitch += Transition_MediaSwitch;
		}

		protected void Start() {
			if (autoBeginTour) {
				BeginTour();
			}
		}

		protected void OnDisable() {
			transition.MediaSwitch -= Transition_MediaSwitch;
		}

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		private void Transition_MediaSwitch(TransitionState state, Node node) {
			if (state == TransitionState.BeforeSwitch) {
				NextNode = node;
				PreviousNode = ActiveNode;
			}
			if (state == TransitionState.Switch) ActiveNode = node;
			MediaSwitch.InvokeSafe(state, node);
		}

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		/// <summary>
		/// Switches the MediaSwitch to the first piece of media.
		/// </summary>
		public void BeginTour() {
			Tour.Current = ActiveTour;
			if (ActiveTour == null) {
				Debug.LogWarning(Warnings.AUTOPLAY_MISSINGTOUR);
				return;
			}
			GoToMedia(entryNode);
		}

		/// <summary>
		/// Instructs the MediaSwitch to move to a given NodeData.
		/// </summary>
		public static void GoToMedia(Node node) {
			if (node == null) {
				Debug.LogWarning(Warnings.GOTOMEDIA_MISSINGDATA);
				return;
			}
			if (Instance.transition == null) {
				Debug.LogWarning(Warnings.MISSING_TRANSITION);
				return;
			}

			if (Instance.transition.IsTransitioning) Instance.transition.Interrupt();
			Instance.transition.StartTransition(node);
		}
	}
}