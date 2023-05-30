using System;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Events;
using TMPro;
using Photon.Pun;

//using System.Collections;
//using System.Collections.Generic;

public class ModalWindowPanel : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject helpPanel;
    
    public static GameObject[] helpPanels;
    public static int helpCount;
    public static GameObject testPanle;
    bool active;



    public void SetActiveOnOff(GameObject gameObject)
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");

        bool isActive = gameObject.activeSelf;
        gameObject.SetActive(!isActive);
    }

    public void OpenAndClosePanel()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");

        helpPanels[helpCount].SetActive(false);
        testPanle.SetActive(false);
        helpCount = 0;
    }

    public void OpenAndSettingsPanel()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");

        if (settingsPanel != null)
        {
           

            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);
        }
    }

    public void HelpPanels()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");

        settingsPanel.SetActive(false);
        helpPanel.SetActive(true);
        helpPanels[0].SetActive(true);

        //Debug.Log(helpPanels.Length);
    }

    public void HelpBack()
    {
        FindObjectOfType<AudioManager>().Play("ExitSFX");

        helpCount--;
        Debug.Log("Count Back " + helpCount);
        if(helpCount <= 0)
        {
            helpPanels[helpCount + 1].SetActive(false);
            testPanle.SetActive(false);
            settingsPanel.SetActive(true);
            helpCount = 0;
        }
        helpPanels[helpCount + 1].SetActive(false);
        helpPanels[helpCount].SetActive(true);
    }

    public void HelpNext()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");

        helpCount++;
        Debug.Log(helpPanels.Length);
        Debug.Log("Count Next " + helpCount);
        helpPanels[helpCount - 1].SetActive(false);
        helpPanels[helpCount].SetActive(true);
    }

    public void ExitGame(){
        FindObjectOfType<AudioManager>().Play("ExitSFX");

        SceneManager.LoadScene("MainMenu");
        if (GameState.sendOverNetwork)
        {
            PhotonNetwork.Disconnect();
        }
    }


}

  