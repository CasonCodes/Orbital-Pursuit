using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public string title;
    public Sprite sprite;
    public string message;
    public bool triggerOnEnable;

    public void OnEnable(){
        if (!triggerOnEnable){ return ;}

        // Action confirmCallback = null;
        // Action declineCallback = null;

        // if (onConfirmEvent.GetPersistentEventCount() > 0){
        //     confirmCallback = onConfirmEvent.Invoke;
        // }
        // if (onDeclineEvent.GetPersistentEventCount() > 0){
        //     declineCallback = onDeclineEvent.Invoke;
        // }

        // UIController.instance.modalWindow.ShowMessage(title, sprite, message, null, null);
    }
}
