using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class TestConnect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    PhotonView pView;

    [SerializeField]
    private TMP_InputField _userName;

    
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Connecting to server...");
        
        //  This function "ConnectUsingSettings" lets photon know
        //  that I want to use the settings described in my app in
        //  the photon website dashboard.
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.NickName = MasterManager.GameSettings.Nickname;
        PhotonNetwork.ConnectUsingSettings();
    }


    //  Called when the client is connected to the Master Server and ready for matchmaking and other tasks.
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected succesfully!");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);

        //We have to join a lobby, we do it as follows:
        PhotonNetwork.JoinLobby();
    }



    //  Called after disconnecting from the Photon server. It could be a failure or an explicit disconnect call
    public override void OnDisconnected(DisconnectCause cause)
    {
        // Destroying instantiated object.
        Debug.Log("Disconnected Succesfully!");
        SceneManager.LoadScene("MainMenu");
    }

    
}
