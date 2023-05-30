using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

using static GameState;
using static Utility;

// IMPORTANT INFORMATION:
// ---------------------------------------------
// CONTROL+M+O collapses all methods


// ---------------------------------------------

public class SnapToGrid : MonoBehaviour
{
    public static float offsetValue = .5f;
    public static Vector3 originalPosition = new Vector3(0,0,0);

    public static Vector3 SnapPiece(GameObject gameObject)
    {
        // obtain position of object
        Vector3 snappedPosition = gameObject.transform.position;

        // determine closest hex tile coordinate by truncating the coordinates of the object
        snappedPosition.x = Mathf.RoundToInt(snappedPosition.x);
        snappedPosition.y = 0f;
        // snappedPosition.y = Mathf.RoundToInt(snappedPosition.y); // dont need to snap y coord - always zero (except beetle)

        // if an even x value --> offset coordinates by the offsetValue
        // offsets the entire column to accomplish hex layout
        if (snappedPosition.x % 2 == 0)
        {
            snappedPosition.z = Mathf.RoundToInt(snappedPosition.z) + offsetValue;
        }
        else
        {
            snappedPosition.z = Mathf.RoundToInt(snappedPosition.z);
        }

        // update object location to fixed coordindate
        gameObject.transform.position = snappedPosition;
        return snappedPosition;
    }

    public void OnMouseDown()
    { 
        originalPosition = gameObject.transform.position;
        Debug.Log("===================================== START OF TURN: [" + howManyTurns + "] =====================================");
        Debug.Log(gameObject.name + " picked up at: " + originalPosition);     
    }

    public void OnMouseUp()        
    {
        Debug.Log(gameObject.name + " dropped at: " + gameObject.transform.position);
        GameController(gameObject, originalPosition, SnapPiece(gameObject));
        //if (fightingAI) TO DO
        //{
        //    Thread.Sleep(GetRandomNumber(100, 250));
        //}
    }   

}
