using UnityEngine;
using UnityEngine.SceneManagement;

namespace DigitalSalmon.C360 {
	public class SceneMenu : Menu {
		[SerializeField]
		protected SceneButton buttonTemplate;
		
		protected override void OnEnable() {
			base.OnEnable();
			ConstructFromSceneManager();
		}

		protected override void OnDisable() {
			base.OnDisable();
			DestroyExistingButtons();
		}

		private void ConstructFromSceneManager() {
			DestroyExistingButtons();

			for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
				SceneButton sceneButton = Instantiate(buttonTemplate, buttonsParent, false);
				string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex( i ) );
				sceneButton.AssignScene(sceneName);
			}
           
		}
	}
}