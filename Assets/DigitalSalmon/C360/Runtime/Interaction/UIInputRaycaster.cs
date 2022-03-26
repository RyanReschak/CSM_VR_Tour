using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DigitalSalmon.C360 {
	public abstract class UIInputRaycaster : BaseBehaviour {
		protected enum Methods {
			Physics,
			Graphic
		}
		//-----------------------------------------------------------------------------------------
		// Events:
		//-----------------------------------------------------------------------------------------

		public event EventHandler InteractableChanged;
		public event FloatEventHandler InteractionAlphaChanged;

		//-----------------------------------------------------------------------------------------
		// Serialized Fields:
		//-----------------------------------------------------------------------------------------
		[Header("Input Raycaster")]
		[SerializeField]
		protected float interactionDelay = 0.25f;

		[SerializeField]
		protected float interactionTime = 1.25f;

		//-----------------------------------------------------------------------------------------
		// Protected Fields:
		//-----------------------------------------------------------------------------------------

		protected LayerMask exclusionLayers = 0; // Layers to exclude from the raycast.
		protected float     rayLength       = 20f; // How far into the scene the ray is cast.

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private GraphicRaycaster[] graphicRaycasters;
		private EventSystem        eventSystem;
		private PointerEventData   pointerEventData;
		private IInteractable      _currentInteractable;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private Sequence            interactionSequence;
		private List<RaycastResult> results;
		private RaycastHit[] physicsResults;

		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public IInteractable CurrentInteractable {
			get => _currentInteractable;
			private set {
				bool changed = _currentInteractable != value;
				_currentInteractable = value;
				if (changed) {
					InteractableChanged?.Invoke();
					interactionSequence.Cancel();
					InteractionAlphaChanged?.Invoke(0);
					if (CurrentInteractable != null) {
						interactionSequence.Coroutine(InteractionCoroutine());
					}
				}
			}
		}

		public IInteractable LastInteractible { get; private set; } //The last interactive item

		public float InteractionTime { get; private set; }
		
		protected virtual Methods Method => Methods.Graphic;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected virtual void Awake() { interactionSequence = new Sequence(this); }

		protected void OnEnable() { Complete360Tour.MediaSwitch += C360_MediaSwitch; }

		protected void Start() { StartCoroutine(RaycastLoopCoroutine()); }

		protected void OnGUI() {
			GUILayout.Label(CurrentInteractable?.ToString());
		}
		
		protected void OnDisable() { Complete360Tour.MediaSwitch -= C360_MediaSwitch; }

		private void C360_MediaSwitch(TransitionState state, Node node) {
			graphicRaycasters = null; // Clear this cache.
		}

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public void ClearRaycasterCache() {
			graphicRaycasters = null;
			LastInteractible = null;
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected IEnumerator InteractionCoroutine() {
			InteractionTime = 0;
			yield return Wait.Seconds(interactionDelay);
			while (true) {
				InteractionTime += UnityEngine.Time.deltaTime;
				if (InteractionTime >= interactionTime) InteractionTime = interactionTime;
				InteractionAlphaChanged?.Invoke(InteractionTime / interactionTime);
				CurrentInteractable.SetInteractionTime(InteractionTime, interactionTime);
				if (InteractionTime == interactionTime) {
					CurrentInteractable.Submit();
					break;
				}

				yield return null;
			}
		}

		protected virtual Vector2 GetInputPosition() { return new Vector2(0.5f,0.5f);
		}
		protected virtual Ray GetRay() { return new Ray();}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private IEnumerator RaycastLoopCoroutine() {
			while (true) {
				yield return new WaitForEndOfFrame();
				switch (Method) {
					case Methods.Physics:
						PhysicsRaycast();
						break;
					case Methods.Graphic:
						GraphicRaycast();
						break;
				}
			}
		}

		private void PhysicsRaycast() {
			const int BUFFER_SIZE = 16;
			if (physicsResults == null) physicsResults = new RaycastHit[BUFFER_SIZE];
			
			int count = Physics.RaycastNonAlloc(GetRay(), physicsResults);
			for (int i = 0; i < count; i++) {
				IInteractable interactable = physicsResults[i].collider.GetComponentInParent<IInteractable>();
				if (interactable == null || !interactable.IsInteractive) continue;
				
				CurrentInteractable = interactable;

				// If we hit an interactive item and it's not the same as the last interactive item, then call Over
				if (interactable != LastInteractible) {
					interactable.BeginInteract();
					DeactiveLastInteractible();
				}

				LastInteractible = interactable;
			}

			if (count == 0) {
				DeactiveLastInteractible();
				CurrentInteractable = null;
			}

		}

		private void GraphicRaycast() {
			//Create a list of Raycast Results
			if (results == null) results = new List<RaycastResult>();

			// Find raycasters.
			if (graphicRaycasters == null || graphicRaycasters.Any(g => g == null)) {
				graphicRaycasters = FindObjectsOfType<GraphicRaycaster>();
			}

			// Find event system.
			if (eventSystem == null) {
				eventSystem = FindObjectOfType<EventSystem>();
				pointerEventData = new PointerEventData(eventSystem);
			}

			//Set the Pointer Event Position to that of the mouse position
			pointerEventData.position = GetInputPosition();

			int hitCount = 0;
			results.Clear();

			foreach (GraphicRaycaster graphicRaycaster in graphicRaycasters) {
				//Raycast using the Graphics Raycaster and mouse click position
				graphicRaycaster.Raycast(pointerEventData, results);

				//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
				foreach (RaycastResult result in results) {
					IInteractable interactable = result.gameObject.GetComponentInParent<IInteractable>();
					if (interactable == null || !interactable.IsInteractive) continue;

					hitCount += 1;

					CurrentInteractable = interactable;

					// If we hit an interactive item and it's not the same as the last interactive item, then call Over
					if (interactable != LastInteractible) {
						interactable.BeginInteract();
						DeactiveLastInteractible();
					}

					LastInteractible = interactable;
				}
			}

			if (hitCount == 0) {
				// Nothing was hit, deactive the last interactive item.
				DeactiveLastInteractible();
				CurrentInteractable = null;
			}
		}

		private void DeactiveLastInteractible() {
			if (LastInteractible == null) return;
			LastInteractible.EndInteract();
			LastInteractible = null;
		}
	}
}