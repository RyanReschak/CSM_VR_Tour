using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SwitchScene : MonoBehaviour
{
    public GameObject videoScreen;

    private VideoPlayer video;
    private IDictionary<int, string> videoSelect;

    void Start()
    {
        
        videoSelect.Add(0, "");
        videoSelect.Add(1, "");
        videoSelect.Add(2, "");
        videoSelect.Add(3, "");

        video = videoScreen.GetComponent<VideoPlayer>();
    }

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
        SceneManager.LoadScene(name);
    }
    public void switchVideo(VideoClip c)
    {
        video.clip = c;
        print("Preparing...");
        try
        {
            video.Prepare();
            while (video.isPrepared);
            print("Video Prepared");
            video.Play();
        } catch
        {
            print("Video Couldn't be played");
        }
        
    }
    /*public void switchVideo(int key)
    {
        video.url = videoSelect[key];
    }*/
 }
