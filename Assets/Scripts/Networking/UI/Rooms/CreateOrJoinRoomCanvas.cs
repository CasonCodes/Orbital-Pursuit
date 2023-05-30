using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoomMenu _roomMenu;

    private RoomsMain _roomsDisplayer;

    public void FirstInitialize(RoomsMain canvases)
    {
        _roomsDisplayer = canvases;     
        _roomMenu.FirstInitialize(canvases);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
