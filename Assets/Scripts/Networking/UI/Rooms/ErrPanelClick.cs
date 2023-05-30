using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrPanelClick : MonoBehaviour
{

    [SerializeField]
    private Button acceptBtn;

    [SerializeField]
    private GameObject usernameErrPanel;

    // Start is called before the first frame update
    void Start()
    {
        acceptBtn.onClick.AddListener(turnPanelsOff);
    }

    public void turnPanelsOff() 
    {
        usernameErrPanel.SetActive(false);
        CreateRoomMenu.errPanel.SetActive(false);
        Debug.Log("Clicking!");
    }
}
