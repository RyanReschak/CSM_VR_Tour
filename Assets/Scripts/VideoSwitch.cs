using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vimeo;

public class VideoSwitch : MonoBehaviour
{
    private Vimeo.Player.VimeoPlayer Video;
    public GameObject MenuShow;
    public GameObject MenuHide;
    // Start is called before the first frame update
    void Start()
    {
        Video = GetComponent<Vimeo.Player.VimeoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Video.GetProgress() >= 0.99f)
        {
            MenuShow.SetActive(true);
            MenuHide.SetActive(false);
        }
    }
}
