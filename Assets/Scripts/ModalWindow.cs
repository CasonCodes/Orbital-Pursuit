using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModalWindow : MonoBehaviour
{
    [SerializeField]
    //Object Prefab
    private GameObject errorCnv;

    // public ModalWindow()
    // {
    //     errorCnv = GameObject.GetComponent<GameObject>();
    // }


    //Temp Object
    private GameObject ErrorTab;

    public void showNetError(string incomingMsg)
    {   
        errorCnv = GameObject.Find("Message Box").GetComponent<GameObject>();

        ErrorTab = (GameObject) Instantiate(errorCnv, new Vector3(0,0,0), Quaternion.identity);
    
        TextMeshProUGUI header = ErrorTab.transform.Find("Message Box/Header/Text (TMP)").GetComponent<TextMeshProUGUI>();

        header.text = incomingMsg;

        ErrorTab.SetActive(true);
    }
}
