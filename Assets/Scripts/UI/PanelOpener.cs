using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject settingsPanelWindow;

    public void settingsPanel()
    {
       if (settingsPanelWindow != null){
        bool isActive = settingsPanelWindow.activeSelf;
        settingsPanelWindow.SetActive(!isActive);
       }
    }
}
