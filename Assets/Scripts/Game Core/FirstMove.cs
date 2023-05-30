using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Utility;
using static GameState;

// IMPORTANT INFORMATION:
// ---------------------------------------------
// CONTROL+M+O collapses all methods


// ---------------------------------------------

public static class FirstMove 
{


    public static string GetKeyAtValue(Vector3 possiblePosition)
    {
        foreach (var entry in board)
        {
            if (entry.Value == possiblePosition)
            {
                return entry.Key;
            }
        }
        return "Piece Name Not Found";
    }


    public static bool TouchingColor(Vector3 position, string color)
    {
        // for every direction
        for (int i = 0; i < 6; i++)
        {
            // obtains coordinate in specified direction
            Vector3 possiblePosition = Look((Direction)i, position);

            // obtain piece name
            //string pieceName = GetKeyAtValue(possiblePosition);

            // if piece name contains specified color
            if (GetTopColor(possiblePosition).Contains(color))
            {
                // the piece at this coordinate is touching the specified color
                return true;
            }
            
        }
        // piece is not touching specified color
        return false;
    }
    public static string GetTopColor(Vector3 position)
    {
        string topColor = "";
        string pieceName = "";

        while (Occupied(position, board))
        {
            // obtain name
            pieceName = GetKeyAtValue(position);
           
            // test name and update top color
            if (pieceName.Contains("White"))
            {
                topColor = "White";
            }
            else
            {
                topColor = "Black";
            }

            // increment y to check for piece on above
            position.y += 0.5f;
        }
        //Debug.Log("Top Color " + topColor);
        return topColor;
    }


    public static List<Vector3> ColorMatch(Dictionary<string, Vector3> board)
    {
        // creates empty list
        List<Vector3> availableStartMoves = new List<Vector3>();

        if (howManyTurns == 1 && whoGoesFirst == "White") 
        {
            availableStartMoves.Add(new Vector3(0, 0, .5f));
        }
        else if (howManyTurns == 1 && whoGoesFirst == "Black")
        {
            availableStartMoves.Add(new Vector3(0, 0, 1.5f)); // n
            availableStartMoves.Add(new Vector3(1, 0, 1)); // ne
            availableStartMoves.Add(new Vector3(1, 0, 0)); // se
            availableStartMoves.Add(new Vector3(0, 0, -.5f)); // s
            availableStartMoves.Add(new Vector3(-1, 0, 0)); // sw
            availableStartMoves.Add(new Vector3(-1, 0, 1)); // nw
        }
        else if (howManyTurns == 2 && whoGoesFirst == "White")
        {
            availableStartMoves.Add(new Vector3 (0, 0, 1.5f)); // n
            availableStartMoves.Add(new Vector3(1, 0, 1)); // ne
            availableStartMoves.Add(new Vector3(1, 0, 0)); // se
            availableStartMoves.Add(new Vector3(0, 0, -.5f)); // s
            availableStartMoves.Add(new Vector3 (-1, 0, 0)); // sw
            availableStartMoves.Add(new Vector3 (-1, 0, 1)); // nw
        }
        else
        {
            // for every entry in dictionary
            foreach (var entry in board)
            {
                // if the entry is a white piece and it is black's turn            
                if (entry.Key.Contains("White") && GetTopColor(entry.Value) == "White" && WhoseTurn() == "White")
                {
                    // for every direction
                    for (int i = 0; i < 6; i++)
                    {
                        // obtain coordinate in that direction
                        Vector3 possiblePosition = Look((Direction)i, entry.Value);

                        // The code below will find the possible moves but since the beetle is
                        // one or more layers above this brings it down
                        possiblePosition.y = 0;

                        // if empty space and coordinate doesnt touch black 
                        if (!Occupied(possiblePosition, board) && !TouchingColor(possiblePosition, "Black"))
                        {
                            // valid first move, add
                            availableStartMoves.Add(possiblePosition);
                        }
                    }
                }
                else if (entry.Key.Contains("Black") && GetTopColor(entry.Value) == "Black" && WhoseTurn() == "Black")
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector3 possiblePosition = Look((Direction)i, entry.Value);
                        possiblePosition.y = 0;

                        if (!Occupied(possiblePosition, board) && !TouchingColor(possiblePosition, "White"))
                        {
                            availableStartMoves.Add(possiblePosition);
                        }
                    }
                }
            }
        }        
        return availableStartMoves;
    }
}
