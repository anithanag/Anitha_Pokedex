using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginScript : MonoBehaviour
{
    public TMP_InputField username, pwd;
    public GameObject optionPanel, loginPanel, popupPanel;

    public void LoginFunction()
    {
        ///Save username in playerpref for further usage
        PlayerPrefs.SetString("u_name", username.text);

        //Validation check
        if (username.text == pwd.text)
        {
            optionPanel.SetActive(true);
            loginPanel.SetActive(false);
        }
        else
        {
            popupPanel.SetActive(true);
        }
    }

    //Reset the inputfield
    public void PopupOK()
    {
        username.text = "";
        pwd.text = "";
    }
}
