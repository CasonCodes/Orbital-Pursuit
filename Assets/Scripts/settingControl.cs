using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class settingControl : MonoBehaviour
{
    public GameObject helpPanel;
    public AudioMixer audioMixer;
    
    private void Start()
    {
        helpPanel.SetActive(true);
        ModalWindowPanel.helpPanels = GameObject.FindGameObjectsWithTag("HelpPanles");
        ModalWindowPanel.testPanle = GameObject.Find("HelpPanels");

        foreach (var panel in ModalWindowPanel.helpPanels)
        {
            panel.SetActive(false);
            //Debug.Log(var.name);
        }

        //Debug.Log(ModalWindowPanel.helpPanels.Length);
        helpPanel.SetActive(false);
        ModalWindowPanel.helpCount = 0;

    }

    public void SetVolume (float volume){
        audioMixer.SetFloat("volume", volume);
    }
}
