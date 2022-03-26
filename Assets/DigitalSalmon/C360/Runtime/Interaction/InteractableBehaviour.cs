using UnityEngine;

namespace DigitalSalmon.C360 {
	public abstract class InteractableBehaviour : BaseBehaviour, IMappedPrefab, IInteractable {
		public event EventHandler Submitted;
		
		private bool _isVisible;
		private bool _isHovered;
		private bool _interactive;
		

		public bool IsVisible {
			get => _isVisible;
			protected set {
				bool oldValue = _isVisible;
				_isVisible = value;
				if (_isVisible != oldValue) OnVisiblityChanged(_isVisible);
			}
		}

		public bool IsHovered {
			get => _isHovered;
			protected set {
				bool oldValue = _isHovered;
				_isHovered = value;
				if (_isHovered != oldValue) OnHoveredChanged(_isHovered);
			}
		}

		public bool Interactive {
			get => _interactive;
			protected set {
				bool oldValue = _interactive;
				_interactive = value;
				if (_interactive != oldValue) OnInteractiveChanged(_interactive);
			}
		}

		protected bool IsTimelineVisible { get; private set; }
		
		protected virtual bool CanBeVisible => IsTimelineVisible;
		protected bool CanBeInteractive { get; private set; }
		
		protected bool UserIsHovering { get; set; }
		protected PrefabElement PrefabElement { get; private set; }

		protected TimelineHelper TimelineHelper { get; private set; }

		protected virtual void Awake() {
			
			TimelineHelper = new TimelineHelper();
		}

		protected virtual void OnEnable() {
			
			TimelineHelper.ActiveChanged += TimelineHelper_ActiveChanged;
			TimelineHelper.ThresholdCrossed += TimelineHelper_ThresholdCrossed;
		}

		protected virtual void Update() {
			TimelineHelper.Update();
			
			IsVisible = CanBeVisible;
			Interactive = IsVisible && CanBeInteractive;
			IsHovered = Interactive && UserIsHovering;
		}

		protected virtual void OnDisable() {
			
			TimelineHelper.ActiveChanged -= TimelineHelper_ActiveChanged;
			TimelineHelper.ThresholdCrossed -= TimelineHelper_ThresholdCrossed;
		}

		private void TimelineHelper_ThresholdCrossed() {
			OnTimelineThresholdCrossed();
		}

		protected virtual void OnTimelineThresholdCrossed() {}
		
		private void TimelineHelper_ActiveChanged() {
			
			IsTimelineVisible = TimelineHelper.IsActive;
		}
		
		protected virtual void ResetState() {
			UserIsHovering = false;
			CanBeInteractive = true;
		}

		protected void DisableInteraction() {
			CanBeInteractive = false;
		}

		protected void EnableInteraction() {
			CanBeInteractive = true;
		}

		protected virtual void OnVisiblityChanged(bool visible) {
			const string VISIBILITY_PARAM = "Visible";
			if (animator != null) animator.SetBool(VISIBILITY_PARAM, visible);
		}
		protected virtual void OnHoveredChanged(bool hovered) { }
		protected virtual void OnInteractiveChanged(bool interactive) { }

		protected virtual void OnInputSubmit() { Submit(); }

		bool IInteractable.IsInteractive => Interactive;

		void IInteractable.BeginInteract() { UserIsHovering = true;  }

		void IInteractable.EndInteract() {  UserIsHovering = false;}

		void IInteractable.Submit() { if (Interactive) OnInputSubmit(); }

		void IInteractable.SetInteractionTime(float t, float totalTime) { OnInteractionTimeChanged(t, totalTime);}
		
		protected virtual void OnInteractionTimeChanged(float t, float totalTime){}
		
		protected void Submit(bool force = false) {
			if (force || Interactive) {
				Submitted?.Invoke();
				OnSubmitted();
			}
		}
		
		protected virtual void OnSubmitted(){}
		void IMappedPrefab.UpdateState(TransitionState state) { }

		void IMappedPrefab.UpdateData(PrefabElement element, Node node) {
			PrefabElement = element;
			TimelineHelper.AssignElement(element);
		}
	}
}