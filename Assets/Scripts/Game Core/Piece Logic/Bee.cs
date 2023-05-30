using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameState;
using static Utility;
using static FirstMove;

public class Bee : MonoBehaviour
{
    public GameObject highlightObject;
    public static List<Vector3> beeLegalMoves;
    public GameObject newParent;
 

    public List<Vector3> SearchForMoves()
    {
        List<Vector3> unoccupied = new List<Vector3>();

        for (int i = 0; i < 6; i++)
        {
            // creates copy of board named simulatedBoard
            Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);

            Direction direction = (Direction)i;
            Vector3 possiblePosition = Look(direction, board[gameObject.name]);                       

            // simulate move
            simulatedBoard[gameObject.name] = possiblePosition;
            
            if (!Occupied(possiblePosition, board) && !Blocked(direction, possiblePosition, board) && !IsPinned(gameObject.name))
            {
                unoccupied.Add(possiblePosition);
                
            }
        }
        return unoccupied; 
    }    


    public void GenerateMoves(GameObject piece)
    {
        if (howManyTurns > 2)
        {
            // If first move and bee is not on board search mopen moves around same color pieces
            if (!OnBoard(piece.name))
            {                
                beeLegalMoves = ColorMatch(board);
            }
            else if (IsPinned(piece.name))
            {
                beeLegalMoves = new List<Vector3>();
            }
            else
            {
                // Find legal moves of bee after places on board
                beeLegalMoves = RemoveHiveBreaks(SearchForMoves(), piece);
                //PrintList(beeLegalMoves, "Bee Leagl moves");
            }
        }
        else
        {
            // cant move bee yet, return list of empty moves
            beeLegalMoves = new List<Vector3>();
        }

    }


    public void OnMouseDown()
    {
       Vector3 originalPosition = gameObject.transform.position;
       GenerateMoves(gameObject);       
       HighlightMoveList(beeLegalMoves, highlightObject);      
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
