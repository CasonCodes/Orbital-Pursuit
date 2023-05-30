using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

using static GameState;
using static Utility;
using static FirstMove;

using static Ant;
using static Bee;
using static Spider;
using static Beetle;
using static Cricket;
using static AI;

// IMPORTANT INFORMATION:
// ---------------------------------------------
// CONTROL+M+O collapses all methods


// ---------------------------------------------

public class AI : MonoBehaviour
{
    public enum Priority
    {
        AttackQueen, DefendQueen, None, MoveQueen
    }

    public enum Difficulty
    {
        Easy = 1, Medium = 2, Hard = 3
    }

    public struct Move
    {
        public string pieceName;
        public Vector3 position;
        public Move(string s, Vector3 v)
        {
            pieceName = s;
            position = v;
        }

    };

    public static Difficulty difficulty;
    public static Priority priority = Priority.None;
    public static Dictionary<string, List<Vector3>> allMoves = new Dictionary<string, List<Vector3>>();





    #region EASY_AI

    private static Move SearchByRandom(Priority priority, Vector3 myBeePosition, Vector3 theirBeePosition)
    {
        List<Move> collectedMoves = new List<Move>();

        if (priority != Priority.None)
        {
            foreach (var piece in allMoves)
            {
                foreach (var position in piece.Value)
                {
                    if (priority == Priority.MoveQueen && piece.Key.Contains("Bee #1") && howManyTurns > 2)
                    {
                        Move chosenMove = new Move(piece.Key, position);
                        collectedMoves.Add(chosenMove);
                    }
                }
            }
        }

        if (collectedMoves.Count > 0)
        {
            int randomIndex = GetRandomNumber(0, collectedMoves.Count);
            return collectedMoves[randomIndex];
        }

        return FindRandomMove();
    }

    public static Move ReturnRandomMove()
    {
        allMoves.Clear();
        Vector3 myBeePosition = new Vector3();
        Vector3 theirBeePosition = new Vector3();

        CollectAllLegalMoves();

        ObtainBeePositions(ref myBeePosition, ref theirBeePosition);
        priority = DeterminePriority(myBeePosition, theirBeePosition);
        return SearchByRandom(priority, myBeePosition, theirBeePosition);
    }

    #endregion





    #region MEDIUM_AI_(MINIMAX)

    public static int Heuristic()
    {
        int score = 0;

        // Number of pieces on the board
        score += board.Count;

        // Number of pieces surrounding the queen bees
        int myQueenSurrounded = HowManySurrounded(GetPiecePosition("Black Logic Tile (Bee #1)"));
        int theirQueenSurrounded = HowManySurrounded(GetPiecePosition("White Logic Tile (Bee #1)"));
        score += (4 - myQueenSurrounded) * 10;
        score -= (4 - theirQueenSurrounded) * 10;

        return score;
    }

    public static int minimax(int depth, bool maximizingPlayer)
    {
        if (depth == 0 || GameOver())
        {
            if (GameOver())
            {
                if (playerTwoWins)
                {
                    return 100000;
                }
                else if (playerOneWins)
                {
                    return -10000;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Heuristic();
            }
        }
        if (maximizingPlayer)
        {
            int bestValue = -10000;
            Dictionary<string, List<Vector3>> allMovesCopy = new Dictionary<string, List<Vector3>>(allMoves);

            foreach (var move in allMovesCopy)
            {
                // Vector3 originalPosition = GetPiecePosition(move.Key);
                foreach (var position in move.Value)
                {
                    Dictionary<string, Vector3> AIsimulatedBoard = new Dictionary<string, Vector3>(board);
                    GameObject piece = GameObject.Find(move.Key);
                    AIsimulatedBoard[piece.name] = position;
                    int value = minimax(depth - 1, false);

                    bestValue = Math.Max(bestValue, value);
                    // RedrawBoard(AIsimulatedBoard);
                }
            }
            return bestValue;
        }
        else
        {
            int bestValue = 10000;
            Dictionary<string, List<Vector3>> allMovesCopy = new Dictionary<string, List<Vector3>>(allMoves);

            foreach (var move in allMovesCopy)
            {

                foreach (var position in move.Value)
                {
                    Dictionary<string, Vector3> AIsimulatedBoard = new Dictionary<string, Vector3>(board);
                    GameObject piece = GameObject.Find(move.Key);
                    AIsimulatedBoard[piece.name] = position;
                    int value = minimax(depth - 1, true);

                    bestValue = Math.Min(bestValue, value);

                }
            }
            return bestValue;
        }
    }

    public static Move ReturnBestMoveMinimax(int depth)
    {
        if (depth == 0)
        {
            // base case - stop recursion           
        }

        allMoves.Clear();
        Move bestMove = new Move();
        Vector3 myBeePosition = new Vector3();
        Vector3 theirBeePosition = new Vector3();

        CollectAllLegalMoves();
        ObtainBeePositions(ref myBeePosition, ref theirBeePosition);
        priority = DeterminePriority(myBeePosition, theirBeePosition);

        foreach (var entry in allMoves)
        {
            foreach (var position in entry.Value)
            {
                Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);
                GameObject piece = GameObject.Find(entry.Key);
                simulatedBoard[piece.name] = position;

                // check for immediate victory
                if (HowManySurrounded(theirBeePosition, simulatedBoard) == 6)
                {
                    bestMove.pieceName = piece.name;
                    bestMove.position = position;
                    return bestMove;
                }
            }
        }

        // only for first layer
        if (priority == Priority.DefendQueen)
        {
            foreach (var entry in allMoves)
            {
                foreach (var position in entry.Value)
                {
                    Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);
                    GameObject piece = GameObject.Find(entry.Key);
                    simulatedBoard[piece.name] = position;

                    // TO DO: check for defense
                    if (HowManySurrounded(myBeePosition, simulatedBoard) < HowManySurrounded(myBeePosition))
                    {
                        bestMove.pieceName = piece.name;
                        bestMove.position = position;
                        return bestMove;
                    }
                }
            }
        }
        // only for first layer, priority is urgent
        else if (priority == Priority.MoveQueen)
        {
            foreach (var entry in allMoves)
            {
                foreach (var position in entry.Value)
                {
                    Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);
                    GameObject piece = GameObject.Find(entry.Key);
                    simulatedBoard[piece.name] = position;

                    // TO DO: check for defense
                    if (HowManySurrounded(myBeePosition, simulatedBoard) < HowManySurrounded(myBeePosition))
                    {
                        bestMove.pieceName = piece.name;
                        bestMove.position = position;
                        return bestMove;
                    }
                }
            }
        }
    









        // if not immmediate victory, keep searching ...


        // recurcive call
        return ReturnBestMoveMinimax(depth - 1);




        //Dictionary<string, Vector3> AIsimulatedBoard = new Dictionary<string, Vector3>(board);
        //GameObject piece = GameObject.Find(move.Key);
        //AIsimulatedBoard[piece.name] = position;

        //int currentValue = minimax(depth, false);

        //if (currentValue > bestValue)
        //{
        //    bestValue = currentValue;
        //    bestMove = new Move(move.Key, position);
        //}
    }

    #endregion






    #region MEDIUM_AI

    // searches for a move based on priority determined.
    // if a move that satisfies the priority is not found, a random move is returned.
    private static Move SearchByPriorityMedium(Priority priority, Vector3 myBeePosition, Vector3 theirBeePosition)
    {

        List<Move> collectedMoves = new List<Move>();

        if (priority != Priority.None)
        {
            foreach (var piece in allMoves)
            {
                foreach (var position in piece.Value)
                {
                    string beeName = "Black Logic Tile (Bee #1)";

                    // creates copy of current board
                    Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);

                    // simulate potential move
                    simulatedBoard[piece.Key] = position;

                    //the # of pieces surrounding the queen vs # of pieces surrounding after potential move
                    if (priority == Priority.DefendQueen && HowManySurrounded(simulatedBoard[beeName], simulatedBoard) < HowManySurrounded(myBeePosition))
                    {
                        Move chosenMove = new Move(piece.Key, position);
                        collectedMoves.Add(chosenMove);
                    }
                    else if (priority == Priority.AttackQueen && TouchesQueen(position, theirBeePosition))
                    {
                        Move chosenMove = new Move(piece.Key, position);
                        collectedMoves.Add(chosenMove);
                    }
                    else if (priority == Priority.MoveQueen && piece.Key.Contains("Bee #1") && howManyTurns > 2)
                    {
                        Move chosenMove = new Move(piece.Key, position);
                        collectedMoves.Add(chosenMove);
                    }

                }
            }
        }

        if (collectedMoves.Count > 0)
        {
            int randomIndex = GetRandomNumber(0, collectedMoves.Count);
            return collectedMoves[randomIndex];
        }

        return FindRandomMove();
    }

    public static Move ReturnMediumMove()
    {
        allMoves.Clear();
        Vector3 myBeePosition = new Vector3();
        Vector3 theirBeePosition = new Vector3();

        CollectAllLegalMoves();

        ObtainBeePositions(ref myBeePosition, ref theirBeePosition);
        priority = DeterminePriority(myBeePosition, theirBeePosition);
        return SearchByPriorityMedium(priority, myBeePosition, theirBeePosition);
    }

    #endregion






    #region HARD_AI

    // loops through all of ai's pieces and collects all possible moves into a Dictionary called allMoves:
    //    <"Ant"    , ( {x,y,z},{x,y,z},{x,y,z} )>
    //    <"Bee"    , ( {x,y,z},{x,y,z},{x,y,z} )>
    //    <"Spider" , ( {x,y,z},{x,y,z}         )>
    //    <"Beetle" , ( {x,y,z},{x,y,z},{x,y,z} )>
    //    <"Cricket", ( {x,y,z},{x,y,z}         )>
    // -------------------------------------------------------------------------------




    // searches for a move based on priority determined.
    // if a move that satisfies the priority is not found, a random move is returned.
    private static Move SearchByPriority(Priority priority, Vector3 myBeePosition, Vector3 theirBeePosition)
    {
        List<Move> collectedMoves = new List<Move>();

        if (priority != Priority.None)
        {
            foreach (var piece in allMoves)
            {
                foreach (var position in piece.Value)
                {
                    string beeName = "Black Logic Tile (Bee #1)";

                    // creates copy of current board
                    Dictionary<string, Vector3> simulatedBoard = new Dictionary<string, Vector3>(board);

                    // simulate potential move
                    simulatedBoard[piece.Key] = position;

                    // the # of pieces surrounding the queen vs # of pieces surrounding after potential move

                    if (priority == Priority.DefendQueen && HowManySurrounded(simulatedBoard[beeName], simulatedBoard) < HowManySurrounded(myBeePosition))
                    {
                        Move chosenMove = new Move(piece.Key, position);
                        collectedMoves.Add(chosenMove);
                    }
                    else if (priority == Priority.AttackQueen && TouchesQueen(position, theirBeePosition)
                        && HowManySurrounded(simulatedBoard["White Logic Tile (Bee #1)"], simulatedBoard) > HowManySurrounded(theirBeePosition))
                    {
                        Move chosenMove = new Move(piece.Key, position);
                        collectedMoves.Add(chosenMove);
                    }
                    else if (priority == Priority.MoveQueen && piece.Key.Contains("Bee #1") && howManyTurns > 2)
                    {
                        Move chosenMove = new Move(piece.Key, position);
                        collectedMoves.Add(chosenMove);
                    }
                }
            }
        }

        if (collectedMoves.Count > 0)
        {
            int randomIndex = GetRandomNumber(0, collectedMoves.Count);
            return collectedMoves[randomIndex];
        }

        // if prioirty is set to defend queen but no defensive move was found, search for attack moves instead
        // if attack moves still arent found, ai will return a random move
        if (priority == Priority.DefendQueen)
        {
            return SearchByPriority(Priority.AttackQueen, myBeePosition, theirBeePosition);
        }

        return FindRandomMove();
    }

    // main easy AI function call
    public static Move ReturnBestMove()
    {
        allMoves.Clear();
        Vector3 myBeePosition = new Vector3();
        Vector3 theirBeePosition = new Vector3();

        CollectAllLegalMoves();

        ObtainBeePositions(ref myBeePosition, ref theirBeePosition);
        priority = DeterminePriority(myBeePosition, theirBeePosition);
        return SearchByPriority(priority, myBeePosition, theirBeePosition);
    }

    #endregion






    #region HELPER_FUNCTIONS

    // returns a Priority enum based on the current game state
    public static Priority DeterminePriority(Vector3 myBeePosition, Vector3 theirBeePosition)
    {
        if (howManyTurns == 6 && !OnBoard("Black Logic Tile (Bee #1)"))
        {
            return Priority.MoveQueen;
        }
        else if (OnBoard("Black Logic Tile (Bee #1)") && HowManySurrounded(myBeePosition) > 3)
        {
            return Priority.DefendQueen;
        }
        else if (OnBoard("White Logic Tile (Bee #1)"))
        {
            return Priority.AttackQueen;
        }
        else
        {
            return Priority.None;
        }

    }

    // returns T/F whether a certain coordinate is adjacent to a bee's position
    public static bool TouchesQueen(Vector3 move, Vector3 beePosition)
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 potentialSurrounding = Look((Direction)i, beePosition);
            if (potentialSurrounding == move)
            {
                return true;
            }
        }
        return false;
    }

    public static void ObtainBeePositions(ref Vector3 myBeePosition, ref Vector3 theirBeePosition)
    {
        myBeePosition = GetPiecePosition("Black Logic Tile (Bee #1)");
        theirBeePosition = GetPiecePosition("White Logic Tile (Bee #1)");
    }

    public static void CollectAllLegalMoves()
    {
        GameObject playerTwosHand = GameObject.Find("Player Two's Hand");
        foreach (Transform child in playerTwosHand.transform)
        {
            // create a new entry for the allMoves dictionary by getting moves from the respective insect script
            KeyValuePair<string, List<Vector3>> entry = ObtainCorrespondingMoveList(child.name);
            allMoves[child.name] = entry.Value;
            //Debug.Log("CHILD NAME " + child.name);
            //Debug.Log("ENTRY NAME " + child.name);
            //PrintList(entry.Value, "ENTRY LIST " + child.name);
            //PrintDictionary(allMoves, "ENTRY ALL MOVES");
        }
    }

    public static Move FindRandomMove()
    {
        Move chosenMove = new Move();
        var rand = new System.Random();
        List<Vector3> moves = new List<Vector3>();

        // choose a random piece
        int pieceIndex;

        do
        {
            pieceIndex = rand.Next(0, allMoves.Count);
        } while (allMoves[GetPieceName(pieceIndex)].Count == 0);

        string pieceName = GetPieceName(pieceIndex);

        // collect all moves for chosen piece
        foreach (var position in allMoves[pieceName])
        {
            moves.Add(position);
        }

        // pick a random move from chosen piece legal moves
        int positionIndex = rand.Next(0, moves.Count);

        // package chosenMove with name and position
        chosenMove.pieceName = pieceName;
        chosenMove.position = moves[positionIndex];
        return chosenMove;
    }

    public static string GetPieceName(int index)
    {
        int count = 0;
        foreach (var entry in allMoves)
        {
            if (count == index)
            {
                return entry.Key;
            }
            count++;
        }
        return "";
    }

    public static bool NoLegalMovesFound()
    {
        foreach (var entry in allMoves)
        {
            if (entry.Value.Count > 0)
            {
                return false;
            }
        }
        return true;
    }

    public static int GetRandomNumber(int min, int max)
    {
        var rand = new System.Random();
        return rand.Next(min, max);
    }

    #endregion





    #region ALPHABETA

    public static int alphabeta(int depth, bool maximizingPlayer, int alpha, int beta)
    {
        if (depth == 0 || GameOver())
        {
            if (GameOver())
            {
                if (playerTwoWins)
                {
                    return 100000;
                }
                else if (playerOneWins)
                {
                    return -10000;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Heuristic();
            }
        }
        if (maximizingPlayer)
        {
            int bestValue = -10000;
            
            Dictionary<string, List<Vector3>> allMovesCopy = new Dictionary<string, List<Vector3>>(allMoves);

            foreach (var move in allMovesCopy)
            {
                foreach (var position in move.Value)
                {
                    Dictionary<string, Vector3> AIsimulatedBoard = new Dictionary<string, Vector3>(board);
                    GameObject piece = GameObject.Find(move.Key);
                    AIsimulatedBoard[piece.name] = position;
                    int value = alphabeta(depth - 1, false, alpha, beta);
                    bestValue = Math.Max(bestValue, value);
                    alpha = Math.Max(alpha, bestValue);

                    if (beta <= alpha)
                    {
                        break; 
                    }
                }
            }
            return bestValue;
        }
        else
        {
            int bestValue = 10000;
            
            Dictionary<string, List<Vector3>> allMovesCopy = new Dictionary<string, List<Vector3>>(allMoves);

            foreach (var move in allMovesCopy)
            {
                foreach (var position in move.Value)
                {
                    Dictionary<string, Vector3> AIsimulatedBoard = new Dictionary<string, Vector3>(board);
                    GameObject piece = GameObject.Find(move.Key);
                    AIsimulatedBoard[piece.name] = position;
                    int value = alphabeta(depth - 1, true, alpha, beta);
                    bestValue = Math.Min(bestValue, value);
                    beta = Math.Min(beta, bestValue);

                    if (beta <= alpha)
                    {
                        break; 
                    }

                }
            }
            return bestValue;
        }
    }

    public static Move ReturnBestMoveAlphaBeta(int depth)
    {
        allMoves.Clear();
        int bestValue = -10000;
        Move bestMove = new Move();
        CollectAllLegalMoves();
        Dictionary<string, List<Vector3>> allMovesCopy = new Dictionary<string, List<Vector3>>(allMoves);
        int alpha = -10000;
        int beta = 10000;

        foreach (var move in allMovesCopy)
        {
           
            foreach (var position in move.Value)
            {
                Dictionary<string, Vector3> AIsimulatedBoard = new Dictionary<string, Vector3>(board);
                GameObject piece = GameObject.Find(move.Key);
                AIsimulatedBoard[piece.name] = position;
                // check depth
                int currentValue = alphabeta(depth, false, alpha, beta);

                if (currentValue > bestValue)
                {
                    bestValue = currentValue;
                    bestMove = new Move(move.Key, position);
                }
                alpha = Math.Max(alpha, bestValue);

                if (beta <= alpha)
                {
                    break; 
                }
            }
        }

        return bestMove;
    }

    #endregion

}

