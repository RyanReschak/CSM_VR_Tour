using DigitalSalmon.C360;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : Button {
	[Header("Scene Button")]
	[SerializeField]
	protected string targetScene;

	public string TargetScene => targetScene;

	protected override void OnSubmitted() {
		base.OnSubmitted();
		DisableInteraction();
			
		FadePostProcess fadePostProcess = FindObjectOfType<FadePostProcess>();
		if (fadePostProcess != null) fadePostProcess.FadeDown(onComplete: () => SceneManager.LoadScene(TargetScene));
		else SceneManager.LoadScene(TargetScene);
	}

	public void AssignScene(string sceneName) {
		this.targetScene = sceneName; 
		SetLabelText(sceneName);
	}
}