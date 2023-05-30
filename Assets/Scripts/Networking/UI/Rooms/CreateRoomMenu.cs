using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{   

    [SerializeField]
    private GameObject hostCamera;

    [SerializeField]
    private GameObject clientCamera;
    
    [SerializeField]
    private GameObject hostSide_Menu;
    
    [SerializeField]
    private GameObject clientSide_Menu;

    [SerializeField]
    private TMP_InputField _roomName;
    
    [SerializeField]
    private TMP_InputField _userName;

    [SerializeField]
    PhotonView pView;
    
    //Error Panels Section:
    [SerializeField]
    GameObject roomFullMsg;
    
    [SerializeField]
    GameObject createRoomErrMsg;

    public GameObject settingsPanel;

    // A reference to the Main Room canvas.
    private RoomsMain _roomsDisplayer;
    
    //Bool variable that indicates whether a player is inside the room on not.
    public static bool inRoom = false;
    
    //Static variable for the panel that presents errors, disabling any interaction with the surroundings.
    public static GameObject errPanel;

    public static bool playerJoined = false;

    public void setErrPanel(bool panel)
    {
        errPanel.SetActive(panel);
    }

    public void FirstInitialize(RoomsMain canvases)
    {
        _roomsDisplayer = canvases;
    }
    
    public void Start()
    {
        //ADDED BY NOAH OVERTON
        FindObjectOfType<AudioManager>().Play("EnemyApproaching");

        errPanel = GameObject.Find("ErrPresenter");
        Debug.Log(errPanel.name);
        errPanel.SetActive(false);
    }

    #region Overloaded MonoBehaviorPunCallbacks
    public override void OnJoinRoomFailed (short returnCode, string message)
    {
        Debug.Log(message);
        roomFullMsg.SetActive(true);
        errPanel.SetActive(true);
    }

    [SerializeField]
    GameObject sndPanel;

    [SerializeField]
    public GameObject awaitPlayerPanel;


    public static GameObject tempAwaitPanel;
    
    public override void OnCreatedRoom()
    {
        FindObjectOfType<AudioManager>().StopPlaying("EnemyApproaching");
        FindObjectOfType<AudioManager>().Play("MainTheme");

        inRoom = true;
        // "this" highlights the created room script...
        _roomsDisplayer.GameplayCanvas.Show();

        // Changing the selected canvas from the Rooms Menu to
        // The Gameplay Menu
        _roomsDisplayer.CreateOrJoinRoomCanvas.Hide();

        hostCamera.SetActive(true);
        clientCamera.SetActive(false);
        hostSide_Menu.SetActive(true);
        
        tempAwaitPanel = awaitPlayerPanel;

        sndPanel.SetActive(true);
        tempAwaitPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        FindObjectOfType<AudioManager>().StopPlaying("EnemyApproaching");
        FindObjectOfType<AudioManager>().Play("MainTheme");

        inRoom = true;
        // "this" highlights the created room script...
        _roomsDisplayer.GameplayCanvas.Show();
        playerJoined = true;

        //awaitingPlayerPanel_TurnOff();
        pView = GameObject.Find("ChatManager").GetComponent<PhotonView>();
        pView.RPC("awaitingPlayerPanel_TurnOff", RpcTarget.Others);

        // Changing the selected canvas from the Rooms Menu to
        // The Gameplay Menu
        _roomsDisplayer.CreateOrJoinRoomCanvas.Hide();

        Debug.Log("JoinedRoom");

        ChatManager chatMngr = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        chatMngr.ChatConnectOnClick(_userName.text);

        hostCamera.SetActive(false);
        clientCamera.SetActive(true);
        clientSide_Menu.SetActive(true);
        //playerDisplay.SetActive(true);

        playerJoined = true;

        GameState.yourColor = "Black";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Creation Failed " + message, this);
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        // Destroying instantiated object.
        if (inRoom)
        {
            Debug.Log("Player " + _userName.text + "has left the room...");

            //Finds the Photon View Component and sends a message throug the chat
            //so the other user sees that the player has left.
            pView = GameObject.Find("ChatManager").GetComponent<PhotonView>();
            pView.RPC("playerLeftMessage", RpcTarget.All, _userName.text);

            errPanel.SetActive(false);
            inRoom = false;
            playerJoined = false;
        }
    }
    #endregion



    #region  OnClick() Functions:
    public void OnClick_CreateRoom()
    {
        if(!string.IsNullOrEmpty(_roomName.text) && !string.IsNullOrEmpty(_userName.text) )
        {
            FindObjectOfType<AudioManager>().Play("PressSFX");

            Debug.Log("Creating room: " + _roomName.text + "!");
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;

            //  JoinOrCreateRoom is used since it works as follows:
            //  If the room is already created it joins, if the room
            //  has not been created it creates it.
            PhotonNetwork.CreateRoom(_roomName.text, options, TypedLobby.Default);
        }
        else
        {
            createRoomErrMsg.SetActive(true);
            errPanel.SetActive(true);
            FindObjectOfType<AudioManager>().Play("InvalidSFX");
        }
    }


    public void LeaveSceneToMainMenu()
    {
        FindObjectOfType<AudioManager>().Play("ExitSFX");

        inRoom = false;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public void RoomFull_OnClick()
    {
        roomFullMsg.SetActive(false);
        errPanel.SetActive(false);
        FindObjectOfType<AudioManager>().Play("PressSFX");
    }

    public void CreateErr_OnClick()
    {
        createRoomErrMsg.SetActive(false);
        errPanel.SetActive(false);
        FindObjectOfType<AudioManager>().Play("PressSFX");
    }

    public void OnClick_RoomsMenuToPlay()
    {
        //pView.RPC("playerLeftMessage", RpcTarget.All,_userName.text);
        inRoom = true;

        //Finds the Photon View Component and sends a message throug the chat
        //so the other user sees that the player has left.
        PhotonNetwork.IsMessageQueueRunning = false;
        if (playerJoined)
        {
            FindObjectOfType<AudioManager>().Play("PressSFX");

            pView = GameObject.Find("ChatManager").GetComponent<PhotonView>();
            pView.RPC("playerLeftMessage", RpcTarget.All, _userName.text);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("InavalidSFX");

            PhotonNetwork.IsMessageQueueRunning = true;
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("MainMenu");
        }
    }


    public void clickSettings(){
        if (settingsPanel != null){
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);
        }
    }

    #endregion

    /**********************************************************************************************************************
     MOUSE ON (HOVER) EVENT ADDED BY NOAH
    ***********************************************************************************************************************/
    public void MouseOn_button()
    {
        FindObjectOfType<AudioManager>().Play("HoverSFX");
    }

}
