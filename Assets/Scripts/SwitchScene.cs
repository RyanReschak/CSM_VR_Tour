using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void nextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void prevScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void switchScene(int index)
    {
        
        SceneManager.LoadScene(index);
    }

    public void switchScene(string name)
    {
        //SceneManager.LoadSceneAsync(name);
        //SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.SetActiveScene();
        SceneManager.LoadScene(name);
    }
}
