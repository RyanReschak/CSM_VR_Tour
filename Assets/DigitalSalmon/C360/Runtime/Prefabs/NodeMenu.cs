using UnityEngine;

namespace DigitalSalmon.C360 {
	public class NodeMenu : Menu {
		[SerializeField]
		protected NodeButton buttonTemplate;

		protected void Start() {
			ConstructFromTour(Complete360Tour.ActiveTour);
		}
		
		protected override void OnEnable() {
			base.OnEnable();
			ConstructFromTour(Complete360Tour.ActiveTour);
		}

		protected override void OnDisable() {
			base.OnDisable();
			DestroyExistingButtons();
		}

		private void ConstructFromTour(Tour tour) {
			DestroyExistingButtons();

			if (tour == null || tour.NodeCollection == null) {
				return;
			}
            
			foreach (Node node in tour.NodeCollection) {
				NodeButton nodeButton = Instantiate(buttonTemplate, buttonsParent, false);
				nodeButton.AssignNode(node);
			}
		}
	}
}