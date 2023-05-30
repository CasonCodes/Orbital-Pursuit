using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class ChatManager : MonoBehaviour
{
    bool isConnected;
    [SerializeField] string username;

    //This function changes the local user's name
    public void UsernameOnValueChange(string valueIn)
    {
        username = valueIn;
    }

    [SerializeField] PhotonView photonView;
    [SerializeField] GameObject chatPanel;
    string currentChat;
    [SerializeField] TMP_InputField chatField;
    [SerializeField] Text chatDisplay;

    public void ChatConnectOnClick(string user)
    {
        isConnected = true;
        if (isConnected)
        {
            // nice
        }
        Debug.Log("Connected to the chat!");
        this.photonView.RPC("joinedChat", RpcTarget.All, "Player " + user + " has joined!");
    }

    public void TypeChatOnValueChange(string valueIn)
    {
        currentChat = chatField.text;
    }

    void Update()
    {
        if (Input.GetKeyDown("return"))
        {
            SubmitPublicChatOnClick();
        }
    }

    public void SubmitPublicChatOnClick()
    {
        if (currentChat == "")
        {
            Debug.Log("Cannot send empty message!");
        }


        //this.photonView.RPC("RPCSubmitChat", RpcTarget.AllBuffered);
        RPCSubmitChat();
        chatField.text = "";
        currentChat = "";
        chatField.Select();
    }


    //[PunRPC]
    void RPCSubmitChat()
    {
        DisplayMessage(username, currentChat);
    }

    [PunRPC]
    void joinedChat(string msg)
    {
        chatDisplay.text += "\n" + msg;
    }

    public void DisplayMessage(string sender, string message)
    {
        if (message != "")
        {
            this.photonView.RPC("RPCDisplay", RpcTarget.AllBuffered, sender, message);
        }
    }

    [PunRPC]
    void RPCDisplay(string sender, string message)
    {
        string msgs = "";
        msgs = string.Format("{0}: {1}", sender, message);
        chatDisplay.text += "\n" + msgs;
        Debug.Log(msgs);
    }


    [PunRPC]
    public void playerLeftMessage(string user)
    {
        chatDisplay.text += "\n" + "Player " + user + " has left the game!";
        playerLeftNotify();
        PhotonNetwork.LeaveRoom();
    }

     
    #region OnleftPanel
    
    [SerializeField]
    GameObject sndPanel;

    [SerializeField]
    GameObject playerLeftPanel;

    public void playerLeftPanel_OnClick()
    {
        playerLeftPanel.SetActive(false);
        //CreateRoomMenu.errPanel.SetActive(false);
        sndPanel.SetActive(false);
        
        PhotonNetwork.IsMessageQueueRunning = true;
        CreateRoomMenu.playerJoined = false;
        SceneManager.LoadScene("MainMenu");

    }

    [PunRPC]
    public void awaitingPlayerPanel_TurnOff()
    {
        CreateRoomMenu.tempAwaitPanel.SetActive(false);
        sndPanel.SetActive(false);
        CreateRoomMenu.playerJoined = true;
    }

    public void playerLeftNotify() 
    {

        CreateRoomMenu.errPanel.SetActive(true);
        sndPanel.SetActive(true);
        playerLeftPanel.SetActive(true);
        Debug.Log("NOTYFYING!");
    }
    #endregion
}
