/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class modalDialog : MonoBehaviour
{
    public static ModalDialog Instance = null;

   public Text dialogText;

   public Button helpButton;
   public Text helpText;

  // public Button resumeButton;
   public Text resumeText;

   public Button exitButton;
   public Text exitText; 

    void Start(){
        Instance = this;
    }

   public void SetDialog (DialogObject d){
    dialogText.text = d.dText;
    helpText.text = d.helpText; 
    resumeText.text = d.resumeText;
    exitText.text = d.exitText;

    helpButton.onClick.RemoveAllListeners();
    helpButton.onClick.AddListeners(d.helpEvent.Invoke); //MOVE TO HELP
 // resumeButton.onClick.RemoveAllListeners();
 // resumeButton.onClick.AddListeners(d.exitEvent.Invoke);  //CONFIRM O CANCEL
    exitButton.onClick.RemoveAllListeners();
    exitButton.onClick.AddListeners(d.exitEvent.Invoke);  //MOVE TO MAIN MENU
   }
}

public class DialogObject {
    public string dText;
    public string resumeText;
    public string helpButton;
    public string exitButton;
    public UnityEvent exitEvent;
    public UnityEvent helpEvent;

}
*/