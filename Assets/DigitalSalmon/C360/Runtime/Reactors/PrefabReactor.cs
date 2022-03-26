using System.Collections.Generic;
using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Core/Prefab Reactor")]
	public class PrefabReactor : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private readonly HashSet<GameObject> spawnedPrefabs = new HashSet<GameObject>();
		private readonly HashSet<IMappedPrefab> activeMappedPrefabs = new HashSet<IMappedPrefab>();

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void OnEnable() { Complete360Tour.MediaSwitch += C360_MediaSwitch; }

		protected void OnDisable() { Complete360Tour.MediaSwitch -= C360_MediaSwitch; }

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		protected void C360_MediaSwitch(TransitionState state, Node node) {
			switch (state) {
				case TransitionState.BeforeSwitch:
					InformPrefabs(state);
					break;
				case TransitionState.Switch:
					DestroyPrefabs();
					if (node != null) CreatePrefabs(node, node.PrefabElements);
					break;
				case TransitionState.AfterSwitch:
					InformPrefabs(state);
					break;
			}
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		/// <summary>
		/// Updates all prefabs of the current MediaSwitchState (Useful for prefab animations).
		/// </summary>
		private void InformPrefabs(TransitionState state) {
			foreach (IMappedPrefab mappedPrefab in activeMappedPrefabs) {
				mappedPrefab.UpdateState(state);
			}
		}

		/// <summary>
		/// Destroys all spawned Prefabs.
		/// </summary>
		private void DestroyPrefabs() {
			foreach (GameObject obj in spawnedPrefabs) {
				Destroy(obj);
			}

			activeMappedPrefabs.Clear();
			spawnedPrefabs.Clear();
		}

		/// <summary>
		/// Creates all prefabs in a given NodeData
		/// </summary>
		private void CreatePrefabs(Node node, IEnumerable<PrefabElement> elements) {
			foreach (PrefabElement element in elements) {
				CreatePrefab(element.Prefab, element, node);
			}
		}

		/// <summary>
		/// Creates a new Prefab and initialises it with a given PrefabElement if required.
		/// </summary>
		private void CreatePrefab(GameObject template, PrefabElement element, Node node) {
			GameObject spawnedPrefab = Instantiate(template, transform, false);
			spawnedPrefab.name = template.name;
			spawnedPrefabs.Add(spawnedPrefab);

			UpdateMappedElementPosition(spawnedPrefab.transform, element);
			spawnedPrefab.transform.Rotate(Vector3.up, 180, Space.Self);
			
			IMappedPrefab[] mappedPrefabComponents = spawnedPrefab.GetComponentsInChildren<IMappedPrefab>();
			foreach (IMappedPrefab component in mappedPrefabComponents) {
				component.UpdateData(element, node);
				activeMappedPrefabs.Add(component);
			}
		}

		private static void UpdateMappedElementPosition(Transform transform, IMappedElement mappedElement, float distance = -1) {
			const float MAPPED_ELEMENT_CAMERA_DISTANCE = 8;
			transform.localPosition = MathUtilities.EquirectangularProjection(new Vector2(mappedElement.Position.x, 1 - mappedElement.Position.y)) * (distance == -1 ? MAPPED_ELEMENT_CAMERA_DISTANCE : distance);
			transform.LookAt(transform.parent);
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
		}
	}
}