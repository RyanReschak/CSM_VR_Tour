using System.Collections.Generic;
using System.Linq;
using DigitalSalmon;
using DigitalSalmon.C360;
using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Transition/Media Transition")]
	public abstract class MediaTransition : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Events:
		//-----------------------------------------------------------------------------------------

		public event EventHandler<TransitionState, Node> MediaSwitch;

		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public bool IsTransitioning { get; protected set; }
		protected bool IsLoading { get; private set; }

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private readonly HashSet<object> loadObjects = new HashSet<object>();

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public abstract void StartTransition(Node node);
		public virtual void Interrupt() { }

		public void RegisterLoadObject(object obj) {
			if (obj == null) return;
			loadObjects.Add(obj);
			IsLoading = true;
		}

		public void UnregisterLoadObject(object obj) {
			if (obj == null) return;
			loadObjects.Remove(obj);
			IsLoading = loadObjects.Any();
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected void InvokeMediaSwitch(TransitionState state, Node node) { MediaSwitch.InvokeSafe(state, node); }
	}
}