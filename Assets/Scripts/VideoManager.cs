using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    private VideoPlayer video;

    private void Awake()
    {
        video = GetComponent<VideoPlayer>();
    }

    public void PlayVideo()
    {
        video.Play();
    }
    public void PauseVideo()
    {
        video.Pause();
    }

    public void Restart()
    {
        video.frame = 0;
        video.time = 0.0F;
    }
}
