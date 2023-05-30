using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameState;
using static Utility;
using static FirstMove;

public class Cricket : MonoBehaviour
{
    public GameObject highlightObject;
    public static List<Vector3> cricketLegalMoves;
    public GameObject newParent;
  

    public Vector3 FindAvailableHop(Direction d, Vector3 startingPosition)
    {
        Vector3 availableHop = startingPosition;
        do
        {
            availableHop = Look(d, availableHop);
        } while (Occupied(availableHop, board));

        return availableHop;
    }

    public List<Vector3> FindOccupied()
    {
        List<Vector3> occupied = new List<Vector3>();
        for (int i = 0; i < 6; i++) 
        {
            Vector3 possiblePosition = Look((Direction)i, GameState.board[gameObject.name]);
            if (Occupied(possiblePosition, board) && !IsPinned(gameObject.name))
            {                
                occupied.Add(FindAvailableHop((Direction)i, possiblePosition));
            }            
        }
        return occupied;
    }

    public void SearchForMoves(GameObject gameObject)
    {
        cricketLegalMoves = RemoveHiveBreaks(FindOccupied(), gameObject);
    }

    public void GenerateMoves(GameObject gameObject)
    {
        string whichBee = "";
        switch (WhoseTurn())
        {
            case "White": whichBee = "White Logic Tile (Bee #1)"; break;
            case "Black": whichBee = "Black Logic Tile (Bee #1)"; break;
        }

        if ((howManyTurns == 7 || howManyTurns == 8) && !OnBoard(whichBee))
        {
            // bee must move by 4th turn
            cricketLegalMoves = new List<Vector3>();
        }
        else if (OnBoard(gameObject.name))
        {
            if (IsPinned(gameObject.name) || !OnBoard(whichBee))
            {
                // piece is pinned and can't move
                cricketLegalMoves = new List<Vector3>();
            }
            else
            {
                // regular search algorithm
                SearchForMoves(gameObject);
            }
        }
        else
        {
            // piece being placed for first time
            cricketLegalMoves = ColorMatch(board);
        }
    }

    public void OnMouseDown()
    {
        GenerateMoves(gameObject);
        HighlightMoveList(cricketLegalMoves, highlightObject);
    }

    public void OnMouseUp()
    {
        DestroyHighlights();
        if (acceptedMove && !sendOverNetwork)
        {
            transform.parent = newParent.transform;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}
