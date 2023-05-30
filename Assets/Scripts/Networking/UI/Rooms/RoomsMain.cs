using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsMain : MonoBehaviour
{
    // This field holds a reference to the CreateOrJoin Canvas.
    [SerializeField]
    private CreateOrJoinRoomCanvas _createOrJoin;
    public CreateOrJoinRoomCanvas CreateOrJoinRoomCanvas {get {return _createOrJoin; }}

    // This field holds a reference to the Gameplay Canvas.
    [SerializeField]
    private GameplayCanvas gameplayRoom;
    public GameplayCanvas GameplayCanvas { get { return gameplayRoom;}}


    private void Awake()
    {
        FirstInitialize();
    }

    public void FirstInitialize()
    {
        // Using this since I am passing this script's reference.
        // Both referencing the accessor which will reference the SerializeFields.
        CreateOrJoinRoomCanvas.FirstInitialize(this);
        GameplayCanvas.FirstInitialize(this);
    }
}
