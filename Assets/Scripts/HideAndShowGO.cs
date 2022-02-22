using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndShowGO : MonoBehaviour
{
    public GameObject startHide;
    public GameObject startShow;

    private void Start()
    {
        hide(startHide);
        show(startShow);
    }

    public void hide(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void show(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void hideAndShow(GameObject objHide, GameObject objShow)
    {
        hide(objHide);
        show(objShow);
    }
}
