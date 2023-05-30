// using System;
// using System.Collections.Generic;
// using System.Windows;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using UnityEngine.Audio;
// using UnityEngine.Events;
// using TMPro;
// //using System.Collections;
// //using System.Collections.Generic;

// public class ModalWindowPanel : MonoBehaviour
// {
//    public GameObject settingsPanel;
//    public GameObject helpPanel;
//    public GameObject introPanel;
//    public GameObject nextOne;
//    public GameObject usingOne;


//    public void OpenAndClosePanel(){
//       if (settingsPanel != null){
//          bool isActive = settingsPanel.activeSelf;
//          settingsPanel.SetActive(!isActive);
//          helpPanel.SetActive(!isActive);
//       }
//     }

//    public void ExitGame(){
//       SceneManager.LoadScene("MainMenu");
//    }

//    public void HelpGame(){
//        if (helpPanel != null){
//          bool isActive =helpPanel.activeSelf;
//          settingsPanel.SetActive(false);
//          helpPanel.SetActive(!isActive);
//          introPanel.SetActive(!isActive);
//       }
//    }
    
//    public void nextWindow(){
//         if (nextOne != null){
//          bool isActive = nextOne.activeSelf;
//          nextOne.SetActive(!isActive);
//          usingOne.SetActive(false);
//       }
//    }
   
//    public void lastWindow(){
//         if (nextOne != null){
//          bool isActive = nextOne.activeSelf;
//          nextOne.SetActive(!isActive);
//          helpPanel.SetActive(false);
//       }
//    }
// }

   
