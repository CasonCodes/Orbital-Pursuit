using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameState;
using static Utility;
using static FirstMove;

public class Beetle : MonoBehaviour
{
    public GameObject highlightObject;
    public static List<Vector3> beetleLegalMoves;
    public GameObject newParent;


    public static float GetStackHeight(Vector3 pos)
    {
        float posHeight = 0.0f;
        while (Occupied(pos, board))
        {
            posHeight += 0.5f;
            pos.y += 0.5f;
        }
        //Debug.Log("Position height: " + posHeight);
        return posHeight;
    }

    public List<Vector3> FindLegalMoves()
    {
        List<Vector3> unoccupied = new List<Vector3>();
        
        for (int i = 0; i < 6; i++)
        {
            
            Direction direction = (Direction)i;
            Vector3 lookedAtPosition = Look(direction, board[gameObject.name]);

            // creates copy of board name simulatedBoard
            Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);

            // simulate move
            simulatedBoard[gameObject.name] = lookedAtPosition;

            if (!Blocked(direction, lookedAtPosition, board) && !IsPinned(gameObject.name) || Occupied(lookedAtPosition, board))
            {
                // Check looked at position height of its the same as the blocked pieces allow jump\
                if (Occupied(lookedAtPosition, board))
                {
                    // If piece infront similate that pieces heigh and checked for freedom to move
                    lookedAtPosition.y = GetStackHeight(lookedAtPosition);
                    if (!Blocked(direction, lookedAtPosition, board))
                    {
                        while (lookedAtPosition.y != 0)
                        {
                            lookedAtPosition.y -= 0.5f;
                        }
                        Vector3 possiblePosition = lookedAtPosition;
                        unoccupied.Add(possiblePosition);
                    }
                }
                else
                {
                    // You always drop the piece on y = 0 so this lowers avalible moves to that coord so you can drop it on there
                    while (lookedAtPosition.y != 0)
                    {
                        lookedAtPosition.y -= 0.5f;
                    }
                    Vector3 possiblePosition = lookedAtPosition;
                    unoccupied.Add(possiblePosition);
                }
            }
        }
        return unoccupied;
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
            beetleLegalMoves = new List<Vector3>();
        }        
        else if (OnBoard(gameObject.name))
        {
            if (IsPinned(gameObject.name) || !OnBoard(whichBee))
            {
                beetleLegalMoves = new List<Vector3>();
            }
            else
            {
                beetleLegalMoves = RemoveHiveBreaks(FindLegalMoves(), gameObject);
                //beetleLegalMoves = FindLegalMoves();
            }
        }
        else
        {
            beetleLegalMoves = ColorMatch(board);
        }
    }

    public void OnMouseDown()
    {
        GenerateMoves(gameObject);
        HighlightMoveList(beetleLegalMoves, highlightObject);
    }

    public void OnMouseUp()
    {
        DestroyHighlights();
        if (acceptedMove && !sendOverNetwork)
        {
            // fix rotation of piece to be parallel with rest of board
            transform.parent = newParent.transform;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

    }
}