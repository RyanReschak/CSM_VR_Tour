using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{

    EventSystem sys;
    public TMP_InputField inputField;
    public Button enterButton;
    public GameObject normalButtons;
    public GameObject capsButtons;
  
    private bool caps;

    // Start is called before the first frame update
    void Start()
    {
        caps = false;
        sys = EventSystem.current;
    }

    private void Update()
    {
       inputField = sys.currentSelectedGameObject.GetComponent<Selectable>().gameObject.GetComponent<TMP_InputField>();
    }

    public void InserChar(string c)
    {
        inputField.text += c;
    }

    public void DeleteChar()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);

        }
    }

    public void InsertSpace()
    {
        inputField.text += " ";
    }

    public void InsertATsign()
    {
        inputField.text += "@";
    }

    public void EnterPressed()
    {
        enterButton.onClick.Invoke();
    }

    public void CapsPressed()
    {
        if (!caps)
        {
            normalButtons.SetActive(false);
            capsButtons.SetActive(true);
            caps = true;
        } else
        {
            capsButtons.SetActive(false);
            normalButtons.SetActive(true);
            caps = false;
        }
    }
}
