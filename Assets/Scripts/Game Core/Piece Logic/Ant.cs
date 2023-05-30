using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameState;
using static Utility;
using static FirstMove;

public class Ant : MonoBehaviour
{
    public GameObject highlightObject;
    public static List<Vector3> antLegalMoves;
    
    public GameObject newParent;

    List<KeyValuePair<Vector3, int>> stepsTaken = new List<KeyValuePair<Vector3, int>>();
    public int sizeOfCurrentLayer = 0;
    public int sizeOfNextLayer = 0;

    void Start() {
        
    }

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


    public void SearchForMoves()
    {
        // initialize steps taken
        stepsTaken.Clear();

        // variable declaration, obtain current position
        int stepsDownPath = 0;
        Vector3 currentPosition = board[gameObject.name];

        // add home position to stepsTaken
        KeyValuePair<Vector3, int> entry = new KeyValuePair<Vector3, int>(currentPosition, stepsDownPath);
        stepsTaken.Add(entry);

        // increment variables because home was added
        stepsDownPath = 1;
        sizeOfCurrentLayer = 1;

        // for every step taken, generate a layer of moves
        for (int i = 0; i < stepsTaken.Count; i++)
        {

            // if not done looping through current layer of moves
            if (sizeOfCurrentLayer > 0)
            {
                // take a step down one path
                FindSurroundingMoves(stepsTaken[i].Key, stepsDownPath);

                // decrement count
                sizeOfCurrentLayer--;
            }
            else if (sizeOfCurrentLayer == 0)
            {
                // make next layer the current layer
                sizeOfCurrentLayer = sizeOfNextLayer;
                sizeOfNextLayer = 0;

                // if done scanning new layer start new layer
                stepsDownPath++;
                FindSurroundingMoves(stepsTaken[i].Key, stepsDownPath);
                // decrement count
                sizeOfCurrentLayer--;
            }
        }

        // add entire layer to spider legal moves
        for (int j = 1; j < stepsTaken.Count; j++)
        {
            antLegalMoves.Add(stepsTaken[j].Key);
        }


        antLegalMoves = RemoveHiveBreaks(antLegalMoves, gameObject);
    }




    public void GenerateMoves(GameObject gameObject)
    {
        

        // GOAL: make user move bee by not highlighting other pieces
        //       (unless your bee is on board, then do nothing different)

        string whichBee = "";
        switch (WhoseTurn())
        {
            case "White": whichBee = "White Logic Tile (Bee #1)"; break;
            case "Black": whichBee = "Black Logic Tile (Bee #1)"; break;
        }

        if ((howManyTurns == 7 || howManyTurns == 8) && !OnBoard(whichBee))
        {
            // bee must move by 4th turn
            antLegalMoves = new List<Vector3>();
        }
        else if (OnBoard(gameObject.name))
        {
            if (IsPinned(gameObject.name) || !OnBoard(whichBee))
            {
                // piece is pinned
                antLegalMoves = new List<Vector3>();
            }
            else
            {
                // regular search algorithm
                SearchForMoves();
            }
        }
        else
        {
            // piece being placed for first time
            antLegalMoves = ColorMatch(board);
        }

    }


    public void OnMouseDown()
    {
        GenerateMoves(gameObject);
        HighlightMoveList(antLegalMoves, highlightObject);
    }

    public void OnMouseUp()
    {
        DestroyHighlights();
        //Debug.Log("Accetped Move" + acceptedMove);
        if (acceptedMove && !sendOverNetwork)
        {
            transform.parent = newParent.transform;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}