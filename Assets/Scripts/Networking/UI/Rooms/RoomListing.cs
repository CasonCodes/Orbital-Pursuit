using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using Photon.Pun;

public class RoomListing: MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    
    [SerializeField]
    private GameObject DialogBox;
    
    [SerializeField]
    private GameObject errPanel;
    
    private string user;
    
    GameObject usernameErrTab;
    DDBScript newtab;

    GameObject errPresenter;
   

    private void Start()
    {
        
        CreateRoomMenu.errPanel.SetActive(true);

        //Object that holds the dialog box, mainly a container.
        errPresenter = GameObject.Find("ErrPresenter");

        usernameErrTab = Instantiate(DialogBox, errPresenter.transform);
        usernameErrTab.SetActive(false);
        
        errPresenter.SetActive(false);
        CreateRoomMenu.errPanel.SetActive(false);
    }

    #region set RoomInfo

    public void setUser(string uName)
    {
        this.user = uName;
    }

    public RoomInfo RoomInfo { get; private set;}

    private RoomInfo temp;

    //  Reversing the string since text mesh reverses the text automatically.
    //  Code taken from: https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
    public string Reverse(string text)
    {
        char[] cArray = text.ToCharArray();
        string reverse = string.Empty;
        for (int i = cArray.Length - 1; i > -1; i--)
        {
            reverse += cArray[i];
        }
        return reverse;
    }

    // Setting the RoomListing object's info by name and maximum amount of players.
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        temp = roomInfo;

        // _userName = GameObject.Find("userInput").GetComponent<TMP_InputField>();
        // + " By " + _userName.text
        
        _text.text = roomInfo.Name;
        
        _text.text = Reverse(_text.text);

        Debug.Log(_text.text);
    }
    #endregion

    #region OnClicks for RoomListings
    public void acceptBtn_OnClick()
    {
        errPresenter = GameObject.Find("ErrPresenter");
        usernameErrTab = GameObject.Find("usernameErrMsg");
        usernameErrTab.SetActive(false);
        //CreateRoomMenu.errPanel.SetActive(false);
        errPresenter.SetActive(false);
    }

    public void OnClick_Button()
    {
        
        if(!string.IsNullOrEmpty(RoomListingsMenu.newUser))
        {
            PhotonNetwork.JoinRoom(temp.Name);
            
        }
        else
        {
            
            usernameErrTab.SetActive(true);
            newtab = usernameErrTab.GetComponent<DDBScript>();

            CreateRoomMenu.errPanel.SetActive(true);
            


            errPresenter.SetActive(true);
            Debug.Log("Cannot enter room without a username!");        
        }
    }
    #endregion
}
