using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using static GameState;
using static SnapToGrid;
using static AI;

// IMPORTANT INFORMATION:
// ---------------------------------------------
// CONTROL+M+O collapses all methods


// ---------------------------------------------

public static class Utility
{

    public enum Direction
    {
        N, NE, SE, S, SW, NW
    }


    #region TILE_CHECKING_METHODS

    // checks to see if any pieces are located at that position     
    public static bool Occupied(Vector3 position, Dictionary<string, Vector3> simulatedBoard)
    {
        foreach (var entry in simulatedBoard)
        {
            if (entry.Value == position)
            {
                return true;
            }
        }
        return false;
    }

    // returns a modified coordinate depending on what Direction is specified     
    public static Vector3 Look(Direction direction, Vector3 position)
    {
        switch (direction)
        {
            case Direction.N:                      position.z += 1.0f; break;
            case Direction.NE: position.x += 1.0f; position.z += 0.5f; break;
            case Direction.SE: position.x += 1.0f; position.z -= 0.5f; break;
            case Direction.S:                      position.z -= 1.0f; break;
            case Direction.SW: position.x -= 1.0f; position.z -= 0.5f; break;
            case Direction.NW: position.x -= 1.0f; position.z += 0.5f; break;
        }
        return position;

        /*==================================================

         (staring down at the board):
         x is which column
         z is which row 
         y is height (only used for beetle stacking)

         (x, y, z) == (column, height, row)

               y
               ^
               |    z
               |   /        * *
               |  /   *   * * *  
               | / * *   * * 
               |/   * * *     
               0----------------> x

        ==================================================*/

    }

    // tests whether the piece at specified position is surrounded by checking in every Direction in clockwise order starting at N     
    public static bool IsAlone(Vector3 position, GameObject piece)
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 lookedAtPosition = Look((Direction)i, position);
            if (Occupied(lookedAtPosition, board) && lookedAtPosition != board[piece.name])
            {                 
                 return false; // doesnt break hive, keep                
            }
        }
        return true; // breaks hive, remove
    }

    // tests whether the piece at specified position is surrounded by checking in every Direction in clockwise order starting at N     
    public static bool IsSurrounded(Vector3 position)
    {
        for (int i = 0; i < 6; i++) // for six directions
        {
            if (!Occupied(Look((Direction)i, position), board))
            {
                //Debug.Log("Stopped directional search for IsSurrounded() - not completely surrounded");
                return false;
            }
        }
        return true;
    }

    public static int HowManySurrounded(Vector3 position)
    { 
        int count = 0;
        for (int i = 0; i < 6; i++) // for six directions
        {
            if (Occupied(Look((Direction)i, position), board))
            {
                count++;             
            }
        }
        return count;
    }
    
    public static int HowManySurrounded(Vector3 position, Dictionary<string, Vector3> simulatedBoard)
    {
        int count = 0;
        for (int i = 0; i < 6; i++) // for six directions
        {
            if (Occupied(Look((Direction)i, position), simulatedBoard))
            {
                count++;             
            }
        }
        return count;
    }

    public static bool OnBoard(string objectName)
    {
        if (board.Count > 0)
        {
            foreach (var entry in board)
            {
                if (entry.Key == objectName)
                {
                    //Debug.Log("OnBoard(): entry.Value: " + entry.Value + " == " + objectName);
                    return true;
                }
            }
        }
        return false;
    }

    public static bool ChangePieceHeight(string pieceName, float height)
    {
        if (OnBoard(pieceName))
        {
            Vector3 position = board[pieceName];
            position.y = height;
            board[pieceName] = position;
            return true;
        }
        else
        {
            Debug.Log("Error in ChangePieceHeight().");
            return false;
        }
    }

    public static bool IsPinned(string pieceName)
    {
        Vector3 piecePos = board[pieceName];
        piecePos.y += 0.5f;
        //Debug.Log("Is pinned " + Occupied(piecePos, board));
        return Occupied(piecePos, board);
    }

    #endregion

    #region HIGHLIGHT_METHODS

    public static void HighlightMove(Vector3 move, GameObject highlightObject)
    {
        // spawn a sphere at pos at list[i]
        highlightObject.transform.position = move;
        Object.Instantiate(highlightObject);
        highlightObject.tag = "Clone";
    }

    public static void HighlightMoveList(List<Vector3> moves, GameObject highlightObject)
    {
        //Debug.Log("Highlighting moves...");

        for (int i = 0; i < moves.Count; i++)
        {
            Vector3 mov = moves[i];
            // spawn a sphere at pos at list[i]
            while (Occupied(mov, board))
            {
                mov.y += .5f;
            }
            highlightObject.transform.position = mov;
            Object.Instantiate(highlightObject);
            highlightObject.tag = "Clone";
        }
    }

    public static void DestroyHighlights()
    {
        //Debug.Log("Destroying highlights...");
        GameObject[] highlightedMoves = GameObject.FindGameObjectsWithTag("Clone");
        foreach (GameObject instance in highlightedMoves)
        {
            Object.Destroy(instance);
        }

    }

    #endregion

    #region DEBUG_METHODS
    
    //public static void PrintList(Dictionary<string, List<Vector3>>)

    // for spider/ant debugging
    public static void PrintList(Dictionary<Vector3, int> list, string title)
    {
        string contents = "CONTENTS OF " + title + "\n\n";
        foreach (var entry in list)
        {
            contents += ("POS: " + entry.Key + " | STEP: " + entry.Value + "\n");
        }
        Debug.Log(contents);
    }


    // prints list of coordinates
    public static void PrintList(List<Vector3> list, string listName)
    {
        string contents = "Contents of " + listName + "\n\n";

        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                contents += (list[i] + "\n");
            }
        }
        Debug.Log(contents);
    }

    public static void PrintList(List<string> list, string listName)
    {
        string contents = "Contents of " + listName + "\n\n";

        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                contents += (list[i] + "\n");
            }
        }
        Debug.Log(contents);
    }

    public static void PrintList(List<KeyValuePair<Vector3, int>> list, string listName)
    {
        string contents = "Contents of " + listName + "\n\n";

        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                contents += (list[i] + "\n");
            }
        }
        Debug.Log(contents);
    }

    public static bool DoesContain(Vector3 position, List<Vector3> list)
    {
        foreach (var entity in list)
        {
            if (position == entity)
            {
                return true;
            }
        }
        return false;
    }    
   
    public static List<Vector3> CopyList(List<Vector3> listToCopy)
    {
        List<Vector3> newList = new List<Vector3>();
        for (int i = 0; i < listToCopy.Count; i++)
        {
            newList.Add(listToCopy[i]);
        }
        return newList;
    }
    public static void PrintDictionary()
    {
        string contents = "[Contents of Dictionary] (Click to expand...)\n\n";
        foreach (KeyValuePair<string, Vector3> entry in board)
        {
            contents += ("Dictionary Entry: " + entry.Key + "\tlocated at: " + board[entry.Key] + "\n"); // debug
        }
        Debug.Log(contents);
    }
    public static void PrintDictionary(Dictionary<string, List<Vector3>> dic, string name)
    {
        string contents = "[Contents of " + name + " ] (Click to expand...)\n\n";
        foreach (var entry in dic)
        {
            contents += ("Dictionary Entry: " + entry.Key);
            foreach (var pos in entry.Value)
            {
                contents += (" " + pos + " "); // debug
            }
            contents += "\n";
        }

        Debug.Log(contents);
    }

    public static string PrintNameAtCord(Vector3 possiblePosition, Dictionary<string, Vector3> simBoard)
    {
        
        foreach (var entry in simBoard)
        {
            if (entry.Value == possiblePosition)
            {
                return entry.Key;
            }
        }
        return "No piece was found";
    }

    #endregion

    #region MISC

    public static int GetRandomNumber(int min, int max)
    {
        var rand = new System.Random();
        return rand.Next(min, max);
    }

    public static void RevertPosition(Vector3 originalPosition, GameObject piece)
    {
        // puts original position back in dictionary
        if (OnBoard(piece.name))
        {
            UpdateDictionaryEntry(piece.name, originalPosition);
        }
        // moves piece on board back to original position
        piece.transform.position = originalPosition;
    }

    public static string GetKeyByValue(Vector3 targetPosition)
    {
        foreach (var entry in allMoves)
        {
            foreach (var move in entry.Value)
            {
                if (move == targetPosition)
                {
                    return entry.Key;
                }
            }
        }
        return "";
    }

    public static void UpdateDictionaryEntry(string pieceName, Vector3 newPosition)
    {
        //Debug.Log("ORIGNAL POS " + newPosition);
        board[pieceName] = newPosition;
        Debug.Log(pieceName + " now located at: " + board[pieceName]);
    }

    public static void RedrawBoard(Dictionary<string, Vector3> board)
    {
        foreach (var entry in board)
        {
            GameObject piece = GameObject.Find(entry.Key);
            piece.transform.position = entry.Value;
        }
    }

    public static Vector3 GetPiecePosition(string pieceName)
    {
        Vector3 position = Vector3.zero;
        if (OnBoard(pieceName))
        {
            position = board[pieceName];
        }
        else
        {
            GameObject gameObject = GameObject.Find(pieceName);
            // position = gameObject.transform.position;
            if (gameObject != null)
            {
                position = gameObject.transform.position;
            }
        }
        return position;
    }

    public static void RemoveDictionaryEntry(string pieceName)
    {
        Debug.Log(pieceName + " removed from dictionary");
        board.Remove(pieceName);
    }

    #endregion

    #region MOVE_VALIDITY_METHODS

    // Checks if the hive will break if you remove piece
    public static bool HiveBreaksWithoutPiece(GameObject piece)
    {
        // Removes peice from board
        Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);
        simulatedBoard.Remove(piece.name);

        var firstEntry = simulatedBoard.First();
        Vector3 firstVector = firstEntry.Value;

        Queue<Vector3> queue = new Queue<Vector3>();
        List<Vector3> visited = new List<Vector3>();

        visited.Clear();
        queue.Enqueue(firstVector);

        while (queue.Count != 0)
        {
            Vector3 positionCheck = queue.Dequeue();

            // Look in every direction and add any peice it finds
            for (int i = 0; i < 6; i++)
            {

                Vector3 lookedAtPosition = Look((Direction)i, positionCheck);

                if (Occupied(lookedAtPosition, simulatedBoard) && !visited.Contains(lookedAtPosition))
                {
                    //Debug.Log("Added " + lookedAtPosition + " to queue");
                    queue.Enqueue(lookedAtPosition);

                }
            }

            visited.Add(positionCheck);
        }

        foreach (var entry in simulatedBoard)
        {
            if (entry.Value.y > 0)
            {
                visited.Add(entry.Value);
            }
        }

        var cleanedList = visited.Distinct();

        //Debug.Log("Clearned COUNT HIVEBREAK " + cleanedList.Count());
        //Debug.Log("VISTED COUNT HIVEBREAK " + visited.Count);
        //Debug.Log("BOARD COUNT HIVEBREAK " + simulatedBoard.Count);

        if (cleanedList.Count() == simulatedBoard.Count)
        {
            //Debug.Log("Hive Break FAlse");
            return false;
        }
        //Debug.Log("Hive Break True");

        return true;
    }

    public static List<Vector3> RoundVectorsInDictionary(List<Vector3> availableMoves)
    {
        Vector3 roundedVector = new Vector3();
        List<Vector3> roundedMoves = new List<Vector3>();
        foreach (Vector3 move in availableMoves)
        {
            float x = Mathf.Round(move.x * 2) / 2;
            float y = Mathf.Round(move.y * 2) / 2;
            float z = Mathf.Round(move.z * 2) / 2;

            roundedVector = new Vector3(x, y, z);
            roundedMoves.Add(roundedVector);
        }
        return roundedMoves;
    }

    // Searching through availiable moves list and removes anything that breaks hive
    public static List<Vector3> RemoveHiveBreaks(List<Vector3> availableMoves, GameObject piece)
    {
        Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);

        Queue<Vector3> queue = new Queue<Vector3>();
        List<Vector3> visited = new List<Vector3>();

        availableMoves = RoundVectorsInDictionary(availableMoves);

        if (!HiveBreaksWithoutPiece(piece))
        {
            
            // for every possible move in the list see if it breaks hive
            for (int x = 0; x < availableMoves.Count; x++)
            {
                simulatedBoard[piece.name] = availableMoves[x];

                visited.Clear();
                queue.Enqueue(availableMoves[x]);

                // Breadth First search number of peices on board
                while (queue.Count != 0)
                {
                    Vector3 positionCheck = queue.Dequeue();
                    
                    // Look in every direction and add any peice it finds
                    for (int i = 0; i < 6; i++)
                    {

                        Vector3 lookedAtPosition = Look((Direction)i, positionCheck);

                        if (Occupied(lookedAtPosition, simulatedBoard) && !visited.Contains(lookedAtPosition))
                        {
                            //Debug.Log("Added " + lookedAtPosition + " to queue");
                            queue.Enqueue(lookedAtPosition);

                        }
                    }

                    visited.Add(positionCheck);
                }

                foreach (var entry in simulatedBoard)
                {
                    if (entry.Value.y > .25)
                    {
                        visited.Add(entry.Value);
                        //Debug.Log("ADDED (Visited): " + entry);
                    }
                }

                if(piece.name.Contains("Beetle") && Occupied(availableMoves[x], board))
                {
                    visited.Add(new Vector3(100, 0, 100));
                }

                var cleanedList = visited.Distinct();

                //Debug.Log("VISITED: " + cleanedList.Count());
                //Debug.Log("PEICES ON BOARD (VISITED): " + simulatedBoard.Count);


                if (cleanedList.Count() != simulatedBoard.Count)
                {
                    availableMoves.RemoveAt(x);
                    x--;
                }

            }
        }
        else
        {
            return new List<Vector3>();
        }

        return availableMoves;
    }

    public static bool TouchesSamePiece(Dictionary<string, Vector3> simulatedBoard, Vector3 prePos, Vector3 newPos)
    {

        List<Vector3> preSurround = new List<Vector3>();
        List<Vector3> newSurround = new List<Vector3>();

        List<string> preNameList = new List<string>();
        List<string> newNameList = new List<string>();

        // Collects all surround coords for previous position
        for (int i = 0; i < 6; i++)
        {
            Direction direction = (Direction)i;
            Vector3 lookedAtPosition = Look(direction, prePos);

            if (Occupied(lookedAtPosition, simulatedBoard))
            {
                preSurround.Add(lookedAtPosition);
            }
        }

        // Collects all surround coords for new position
        for (int i = 0; i < 6; i++)
        {
            Direction direction = (Direction)i;
            Vector3 lookedAtPosition = Look(direction, newPos);

            if (Occupied(lookedAtPosition, simulatedBoard))
            {
                newSurround.Add(lookedAtPosition);
            }
        }

        foreach (var coord in preSurround)
        {
            if (newSurround.Contains(coord))
            {
                return true;
            }
        }

        //PrintList(preSurround, "PRESURROUNDED");
        foreach(var entry in preSurround)
        {
            preNameList.Add(PrintNameAtCord(entry, simulatedBoard));
        }
        //PrintList(preNameList, "PreSurroundedList");
        //PrintList(newSurround, "newSURROUNDED");
        foreach (var entry in newSurround)
        {
            newNameList.Add(PrintNameAtCord(entry, simulatedBoard));
        }
        //PrintList(newNameList, "NewSurroundedList");
        return false;
    }

    public static bool BeenHereBefore(Vector3 potentialMove, List<Vector3> pastMoves)
    {
        for (int i = 0; i < pastMoves.Count; i++)
        {
            if (potentialMove == pastMoves[i])
            {
                return true;
            }
        }
        return false;
    }


    public static bool Blocked(Direction direction, Vector3 originalPosition, Dictionary<string, Vector3> simulatedBoard)
    {
        Vector3 NW = Look(Direction.NW, originalPosition);
        Vector3 NE = Look(Direction.NE, originalPosition);
        Vector3 N = Look(Direction.N, originalPosition);
        Vector3 S = Look(Direction.S, originalPosition);
        Vector3 SE = Look(Direction.SE, originalPosition);
        Vector3 SW = Look(Direction.SW, originalPosition);

        bool isBlocked = false;
        Vector3 destination = new Vector3();

        //Debug.Log("Original Positon: " + originalPosition);

        switch (direction)
        {
            case Direction.N:
                destination = N;
                isBlocked = Occupied(SW, simulatedBoard) && Occupied(SE, simulatedBoard);
                break;
            case Direction.NE:
                destination = NE;
                isBlocked = Occupied(S, simulatedBoard) && Occupied(NW, simulatedBoard);
                break;
            case Direction.SE:
                destination = SE;
                isBlocked = Occupied(N, simulatedBoard) && Occupied(SW, simulatedBoard);
                break;
            case Direction.S:
                destination = S;
                isBlocked = Occupied(NE, simulatedBoard) && Occupied(NW, simulatedBoard);
                break;
            case Direction.SW:
                destination = SW;
                isBlocked = Occupied(N, simulatedBoard) && Occupied(SE, simulatedBoard);
                break;
            case Direction.NW:
                destination = NW;
                isBlocked = Occupied(S, simulatedBoard) && Occupied(NE, simulatedBoard);
                break;
        }

        return isBlocked;
    }

    public static bool IsValidMove(Vector3 newPosition, List<Vector3> legalMoves, GameObject piece, Direction direction, Vector3 curPos)
    {

        // creates copy of board name simulatedBoard
        Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);

        // record original position
        Vector3 originalPosition = board[piece.name];

        // simulate move
        simulatedBoard[piece.name] = newPosition;

        //Debug.Log("IsVAlid Trying newPost at " + newPosition);
        //Debug.Log("IsValid Direction " + direction);
        //Debug.Log("IsVAlid Trying curPos at " + curPos);
        //Debug.Log("IsValid TouchesSame Piece " + TouchesSamePiece(simulatedBoard, curPos, newPosition));
        //Debug.Log("IsValid BeenHerebefore " + BeenHereBefore(newPosition, legalMoves));
        //Debug.Log("IsValid Blocked " + Blocked(direction, newPosition, simulatedBoard));
        //Debug.Log("IsValid OnBoard " + OnBoard(piece.name));

        return            
            TouchesSamePiece(simulatedBoard, curPos , newPosition) &&
            !BeenHereBefore(newPosition, legalMoves) &&
            !Blocked(direction, newPosition, simulatedBoard);
    }

    #endregion
}
