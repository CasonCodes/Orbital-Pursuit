using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DDBScript : MonoBehaviour
{



    [Header ("Header")]
    [SerializeField] 
    private Transform _HeaderArea;
    [SerializeField]
    private TextMeshProUGUI _titleField;

    [Header ("Content")]
    [SerializeField]
    private Transform _contentArea;
    [SerializeField]
    private Transform _verticalLayoutArea;
    [SerializeField]
    private Image _contentImage;
    [SerializeField]
    private TextMeshProUGUI _contentText;

    [Header ("Footer")]
    [SerializeField]
    private Transform _footerArea;
    [SerializeField]
    private Button _confirmButton;
    [SerializeField]
    private Button _cancelButton;

   /* 
    private Action onConfirmAction;
    private Action onDeclineAction;

    public void Confirm()
    {
        onConfirmAction?.Invoke();
        Close();
    }

     public void Decline()
    {
        onDeclineAction?.Invoke();
        Close();
    }
*/ 
    public void ShowMessage (string title, string message, bool confirmBtn, bool exitBtn){
        // bool hasTitle = string.IsNullOrEmpty(title);
        _HeaderArea.gameObject.SetActive(true);
        _titleField.text = title; 
        _contentText.text = message;

        if(confirmBtn)
        {
            Debug.Log("Activating Btn!");
            _confirmButton.gameObject.SetActive(confirmBtn);
        }
        
        if(!exitBtn)
        {
            Debug.Log("Should not happen!");
            Destroy(_cancelButton.gameObject);
        }
        
        if(exitBtn)
        {
            Debug.Log("Activating Btn!");
            _cancelButton.gameObject.SetActive(exitBtn);
        }
        
        if(!confirmBtn)
        {
            Debug.Log("Should not happen!");
            Destroy(_confirmButton.gameObject);
        }
        
        // this.SetActive(true);

    }

    public void ShowMessage (string title, Sprite imageToShow, string message, Action confirmAction, Action declineAction){
        bool hasTitle = string.IsNullOrEmpty(title);
        _HeaderArea.gameObject.SetActive(hasTitle);
        _titleField.text = title; 

        // _contentImage.sprite = imageToShow;
        // _contentText.text = message;

        // onConfirmCallback = confirmAction;
        // onDeclineCallback = declineAction;

        // this.SetActive(true);


    }

    
}
