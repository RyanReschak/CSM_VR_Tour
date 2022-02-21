using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSwitch : MonoBehaviour
{
    private VideoPlayer video;
    public string scene_switch;
    // Start is called before the first frame update
    void Start()
    {
        video = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if ((long)video.frame >= (long)(video.frameCount - 1))
        {
            
            SceneManager.LoadScene(scene_switch);
        }
    }
}
