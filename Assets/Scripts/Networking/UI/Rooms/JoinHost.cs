using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class JoinHost : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private PlayerListing _newClient;

    private GameObject _enlistedClient;

    public override void OnPlayerEnteredRoom(Player newClientPlayer)
    {
        _enlistedClient.SetActive(true);
        _newClient.SetPlayerInfo(newClientPlayer);
    }

    public void OnPlayerLeftRoom()
    {
        // Destroying instantiated object.
        Debug.Log("Player has left the room...");
        _enlistedClient.SetActive(false);
        PhotonView pView = GameObject.Find("ChatManager").GetComponent<PhotonView>();

        pView.RPC("playerLeftMessage", RpcTarget.All);
    }

}
