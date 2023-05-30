using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCanvas : MonoBehaviour
{
    private RoomsMain _roomsDisplayer;

    public void FirstInitialize(RoomsMain canvases)
    {
        _roomsDisplayer = canvases;     
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
