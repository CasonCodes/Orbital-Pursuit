using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private RoomListing _roomListing;

    [SerializeField]
    private TMP_InputField _userName;

    public static string newUser;

    RoomListing listing;

    private List<RoomListing> _listings = new List<RoomListing>(); 
    
    public void UserNameOnValueChange(string newUName)
    {
        newUser = newUName;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove from room list.
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            // Add to room list
            else
            {
                listing = Instantiate(_roomListing, _content);
                
                if(listing != null){
                    listing.SetRoomInfo(info);
                    _listings.Add(listing);
                }
            }
        }
    }

}
