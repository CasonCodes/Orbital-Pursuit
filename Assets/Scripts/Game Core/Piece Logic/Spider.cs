using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameState;
using static Utility;
using static FirstMove;

public class Spider : MonoBehaviour
{
    public GameObject highlightObject;
    public static List<Vector3> spiderLegalMoves;
    List<KeyValuePair<Vector3, int>> stepsTaken = new List<KeyValuePair<Vector3, int>>();
    public int sizeOfCurrentLayer = 0;
    public int sizeOfNextLayer = 0;
    public GameObject newParent;
 

    public List<Vector3> ExtractList()
    {
        List<Vector3> list = new List<Vector3>();
        for (int i = 0; i < stepsTaken.Count; i++)
        {
            list.Add(stepsTaken[i].Key);
        }
        return list;
    }

    public void FindSurroundingMoves(Vector3 currentPosition, int stepsDownPath)
    {
        for (int i = 0; i < 6; i++)
        {
            Direction direction = (Direction)i;
            Vector3 lookedAtPosition = Look(direction, currentPosition);
            if (!Occupied(lookedAtPosition, board) && !IsPinned(gameObject.name) && IsValidMove(lookedAtPosition, ExtractList(), gameObject, direction, currentPosition))
            {
                KeyValuePair<Vector3, int> entry = new KeyValuePair<Vector3, int>(lookedAtPosition, stepsDownPath);
                stepsTaken.Add(entry);
                
                sizeOfNextLayer++;                
            }
        }
    }

    public void SearchForMoves(GameObject gameObject)
    {
        spiderLegalMoves.Clear();
        stepsTaken.Clear();

        int stepsDownPath = 0;
        Vector3 currentPosition = board[gameObject.name];

        KeyValuePair<Vector3, int> entry = new KeyValuePair<Vector3, int>(currentPosition, stepsDownPath);
        stepsTaken.Add(entry);

        stepsDownPath = 1;
        sizeOfCurrentLayer = 1;

        for (int i = 0; i < stepsTaken.Count; i++)
        {
            if (sizeOfCurrentLayer > 0)
            {
                FindSurroundingMoves(stepsTaken[i].Key, stepsDownPath);
                sizeOfCurrentLayer--;
            }
            else if (sizeOfCurrentLayer == 0)
            {
                sizeOfCurrentLayer = sizeOfNextLayer;
                sizeOfNextLayer = 0;

                if (stepsDownPath == 3)
                {
                    for (int j = 0; j < stepsTaken.Count; j++)
                    {
                        if (stepsTaken[j].Value == 3)
                        {
                            spiderLegalMoves.Add(stepsTaken[j].Key);
                        }
                    }

                    break;
                }

                stepsDownPath++;
                FindSurroundingMoves(stepsTaken[i].Key, stepsDownPath);
                sizeOfCurrentLayer--;
            }
        }
        //PrintList(stepsTaken, "STEpSTaken");

        //for (int j = 1; j < stepsTaken.Count; j++)
        //{
        //    spiderLegalMoves.Add(stepsTaken[j].Key);
        //}


        // Remove Hive Breaks
        spiderLegalMoves = RemoveHiveBreaks(spiderLegalMoves, gameObject);
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
            spiderLegalMoves = new List<Vector3>();
        }
        else if (OnBoard(gameObject.name))
        {
            if (IsPinned(gameObject.name) || !OnBoard(whichBee))
            {
                // piece is pinned and can't move
                spiderLegalMoves = new List<Vector3>();
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
            spiderLegalMoves = ColorMatch(board);
        }
    }

    public void OnMouseDown()
    {
        GenerateMoves(gameObject);
        HighlightMoveList(spiderLegalMoves, highlightObject);        
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
