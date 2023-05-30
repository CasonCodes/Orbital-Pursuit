using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Photon.Pun;
using TMPro;
using static Ant;
using static Bee;
using static Spider;
using static Beetle;
using static Cricket;

using static SnapToGrid;
using static Utility;
using static AI;
using static TalkingAI;

// IMPORTANT INFORMATION:
// ---------------------------------------------
// CONTROL+M+O collapses all methods

// even turns: ai (black)
// odd turns: user (white) 


// ---------------------------------------------

public class GameState : MonoBehaviour
{
    public static Dictionary<string, Vector3> board;
    public static Vector3 originalPosition;
    public static List<Vector3> legalMoves;
    public static int howManyTurns;  
    public static bool fightingAI;
    public static bool aiTurn;
    public static string whoGoesFirst;
    public static bool acceptedMove;
    public static bool sendOverNetwork;
    public static string yourColor = "White";
    public static bool playerOneWins;
    public static bool playerTwoWins;
    public static int aiPhraseCounter;
    public static Dictionary<string, List<Vector3>> currentTurnMoves;

    void Start()
    {
        // initialize game board and initialize variables
        board = new Dictionary<string, Vector3>();

        // create a new dictionary to gold the current turns moves so each
        // insect only has to run their search algorithm once to avoid bugs
        currentTurnMoves = new Dictionary<string, List<Vector3>>();

        // initialize other variables
        //whoGoesFirst = "White";
        //yourColor = "White";
        playerOneWins = false;
        playerTwoWins = false;
        aiPhraseCounter = 0;

        // if fightingAI true --> 2 player
        // if fightingAI false --> fighting AI
        Debug.Log("Fighting AI " + fightingAI);

        if (fightingAI)
        {
            // player 2's colliders will never be used when fighting the ai
            DisablePlayerHand(2);

            // disable multiplayer variable so networking functions aren't executed
            sendOverNetwork = false;

            // send ai's pieces off screen so user doesn't see them
            GameObject playersHand = GameObject.Find("Player Two's Hand");
            Vector3 newPosition = playersHand.transform.position;
            newPosition.y = 500;
            playersHand.transform.position = newPosition;
        }

        if (whoGoesFirst == "Black" && fightingAI)
        {
            // set howManyTurns to 0 so the game starts on an even turn number            
            howManyTurns = 0;
            
            // ai opening move
            GameObject piece = GameObject.Find("Black Logic Tile (Cricket #1)");
            legalMoves = new List<Vector3>();
            Vector3 openingMove = new Vector3(0, 0, .5f);
            legalMoves.Add(openingMove);
            GameController(piece, GetPiecePosition(piece.name), openingMove);
        }
        else
        {
            // set howManyTurns to 1 so the game starts on an odd turn number
            howManyTurns = 1;
            // whoGoesFirst = "White"; // whoGoesFirst should already be set to white
        }
     }





    #region GAME_FLOW_LOGIC

    public static string WhoseTurn()
    {
        switch (howManyTurns % 2)
        {
            case 0: return "Black"; // even turns: ai (black)
            default: return "White"; // odd turns: user (white) 
        }
    }

    public static void HandleMove(GameObject piece, Vector3 originalPosition, Vector3 newPosition)
    {
        // if (the piece you are touching is NOT your color) OR (it is not your turn) --> revert move 
        if (!piece.name.Contains(yourColor) && !fightingAI || WhoseTurn() != yourColor && !fightingAI)
        {
            // sound effect for placing piece on board
            FindObjectOfType<AudioManager>().Play("InvalidSFX");

            Debug.Log("It is not your turn or it is not your piece, reverting...");
            RevertPosition(originalPosition, piece);
            acceptedMove = false;
            Debug.Log("[TURN REJECTED]");
            sendOverNetwork = false;
    
        }
        else if (UserMadeLegalMove(newPosition, legalMoves))
        {
            // sound effect for placing piece on board
            FindObjectOfType<AudioManager>().Play("PieceDownSFX");

            if (piece.name.Contains("Beetle"))
            {
                // count height of destination
                float stackHeight = GetStackHeight(newPosition);

                if (stackHeight > 0 && stackHeight <= 2)
                {
                    // change height to y + 1
                    ChangePieceHeight(piece.name, stackHeight + 0.5f);
                    newPosition.y = stackHeight;
                }
            }

            Debug.Log(piece.name + " was dropped at a legal position!");
            UpdateDictionaryEntry(piece.name, newPosition);

            howManyTurns++;
            acceptedMove = true;
            Debug.Log("[TURN ACCEPTED]");

            if (!fightingAI)
            {
                sendOverNetwork = true;
            }

            // Sends move over network https://subscription.packtpub.com/book/game-development/9781849692328/2/ch02lvl1sec26/using-photonviews
            if (sendOverNetwork)
            {
                PhotonView photonView = PhotonView.Get(piece);
                photonView.RPC("RecieveMove", RpcTarget.All, piece.name, newPosition, howManyTurns);
            }
            
            sendOverNetwork = false;
           
        }
        else
        {
            // sound effect for placing piece on board
            FindObjectOfType<AudioManager>().Play("InvalidSFX");

            Debug.Log(piece.name + " was dropped at an illegal position, reverting...");
            RevertPosition(originalPosition, piece);

            acceptedMove = false;
            Debug.Log("[TURN REJECTED]");
            sendOverNetwork = false;

            if (fightingAI)
            {
                antLegalMoves.Clear();
                beeLegalMoves.Clear();
                cricketLegalMoves.Clear();
                spiderLegalMoves.Clear();
                beetleLegalMoves.Clear();
            }
        }
        if (fightingAI)
        {
            legalMoves.Clear();
        }
        
    }

    public static void GameController(GameObject piece, Vector3 originalPosition, Vector3 newPosition)
    {
        if (WhoseTurn() == "White")
        {
            // handle player 1's move
            ObtainCorrespondingMoveList(piece.name);
            HandleMove(piece, originalPosition, newPosition);
        }
        else if (WhoseTurn() == "Black")
        {
            if (fightingAI)
            {
                // handle AI move
                HandleMove(piece, originalPosition, newPosition);
                aiTurn = false;
                allMoves.Clear();
            }
            else
            {
                // handle player 2's move
                ObtainCorrespondingMoveList(piece.name);
                HandleMove(piece, originalPosition, newPosition);

            }
        }

        RedrawBoard(board);

        if (!GameOver() && fightingAI && WhoseTurn() == "Black")
        {
            Debug.Log("===================================== START OF TURN: [" + howManyTurns + "] =====================================");

            aiTurn = true;           
            Move chosenMove = new Move();

            switch (difficulty)
            {
                case Difficulty.Easy: chosenMove = ReturnRandomMove(); break;
                case Difficulty.Medium: chosenMove = ReturnMediumMove() ;  break;
                case Difficulty.Hard: chosenMove = ReturnBestMove();  break;
            }

            if (chosenMove.pieceName == "No Moves Found") // TO DO
            {
                // skip turn, no moves found
                howManyTurns++;
                aiTurn = false;
                legalMoves.Clear();
            }
            else
            {          
                Debug.Log("AI :: [Priority = " + priority + "] Chosen move: " + chosenMove.pieceName + " --> " + chosenMove.position);
                GameObject chosenPiece = GameObject.Find(chosenMove.pieceName);
                legalMoves.Add(chosenMove.position);
                GameController(chosenPiece, GetPiecePosition(chosenMove.pieceName), chosenMove.position);
            }
            
        }
    }
    
    public static bool GameOver()
    {
        // determine if either player won
        if (OnBoard("White Logic Tile (Bee #1)") && IsSurrounded(board["White Logic Tile (Bee #1)"]))
        {
            playerTwoWins = true;
        }
        if (OnBoard("Black Logic Tile (Bee #1)") && IsSurrounded(board["Black Logic Tile (Bee #1)"]))
        {
            playerOneWins = true;
        }

        // generate a final phrase for the ai
        if (fightingAI)
        {
            AiTalk();
        }
        

        // display end of game pop up window
        if (playerOneWins && playerTwoWins)
        {
            Debug.Log("Game Over -- Draw!");
            EndGameHandler.EndGame("Draw");
        }
        else if (playerOneWins)
        {
            Debug.Log("Game Over -- White Wins!");
            EndGameHandler.EndGame("PlayerOneWins");
        }
        else if (playerTwoWins)
        {
            Debug.Log("Game Over -- Black Wins!");
            EndGameHandler.EndGame("PlayerTwoWins");
        }

        // if either player has won, the game is over
        if (playerOneWins || playerTwoWins)
        {
            DisableBoard();
            DisablePlayerHand(1);
            DisablePlayerHand(2);
            return true;
        }

        // if neither player has won, game is not over
        return false;
    }

    public static KeyValuePair<string, List<Vector3>> ObtainCorrespondingMoveList(string pieceName)
    {
        GameObject piece = GameObject.Find(pieceName);

        if (pieceName.Contains("Bee") && !pieceName.Contains("Beetle"))
        {
            if (aiTurn == true)
            {
                Bee beeScript = piece.GetComponent<Bee>();
                beeScript.GenerateMoves(piece);
            }
            legalMoves = new List<Vector3>(beeLegalMoves);
            beeLegalMoves.Clear();
        }
        else if (pieceName.Contains("Ant"))
        {
            if (aiTurn == true)
            {
                Ant antScript = piece.GetComponent<Ant>();
                antScript.GenerateMoves(piece);
            }
            legalMoves = new List<Vector3>(antLegalMoves);
            antLegalMoves.Clear();
        }
        else if (pieceName.Contains("Spider"))
        {
            if (aiTurn == true)
            {
                Spider spiderScript = piece.GetComponent<Spider>();
                spiderScript.GenerateMoves(piece);
            }
            legalMoves = new List<Vector3>(spiderLegalMoves);
            spiderLegalMoves.Clear();
        }
        else if (pieceName.Contains("Beetle"))
        {
            if (aiTurn == true)
            {
                Beetle beetleScript = piece.GetComponent<Beetle>();
                beetleScript.GenerateMoves(piece);
            }
            legalMoves = new List<Vector3>(beetleLegalMoves);
            beetleLegalMoves.Clear();
        }
        else if (pieceName.Contains("Cricket"))
        {
            if (aiTurn == true)
            {
                Cricket cricketScript = piece.GetComponent<Cricket>();
                cricketScript.GenerateMoves(piece);
            }
            legalMoves = new List<Vector3>(cricketLegalMoves);
            cricketLegalMoves.Clear();
        }


        //HighlightMoveList(legalMoves, highlightObject);

        KeyValuePair<string, List<Vector3>> newEntryForAI =
                new KeyValuePair<string, List<Vector3>>(pieceName, legalMoves);
        return newEntryForAI;
    }

    #endregion





    #region PLAYER_TURN_ENFORCEMENT

    public static void DisablePlayerHand(int whichPlayer)
    {
        GameObject playersHand = GameObject.Find("Player One's Hand");

        switch (whichPlayer)
        {
            case 1: playersHand = GameObject.Find("Player One's Hand"); break;
            case 2: playersHand = GameObject.Find("Player Two's Hand"); break;
        }


        // loops through game pieces and disables colliders so user cannot click
        foreach (Transform child in playersHand.transform)
        {
            Collider c = child.GetComponent<Collider>();
            c.enabled = false;
        }
    }

    public static void DisableBoard()
    {
        GameObject Board = GameObject.Find("OnBoard");


        // loops through game pieces and disables colliders so user cannot click
        foreach (Transform child in Board.transform)
        {
            Collider c = child.GetComponent<Collider>();
            c.enabled = false;
        }
    }

    public static bool UserMadeLegalMove(Vector3 newPosition, List<Vector3> legalMoves)
    {
        return DoesContain(newPosition, legalMoves);
    }

    #endregion





    #region NETWORKING_LOGIC

    // Passes move to player over network
    // RPC cannot be static? https://doc.photonengine.com/pun/current/gameplay/rpcsandraiseevent
    [PunRPC]
    public void RecieveMove(string pieceName, Vector3 newPosition, int turnCounter)
    {
        howManyTurns = turnCounter;
        //Debug.Log("Sent " + pieceName + newPosition);
        TransformPiece(pieceName);
        UpdateDictionaryEntry(pieceName, newPosition);
        RedrawBoard(board);
        GameOver();

        //Debug.Log("HOW MANY TURNS: " + howManyTurns);

        // TO DO: Update dictionary
        // TO DO: Redraw board
        // TO DO: Switch Turns
    }

    public static void TransformPiece(string pieceName)
    {
        var fooGroup = FindObjectsOfType<GameObject>(true);

        GameObject piece = GameObject.Find(pieceName);
        foreach (var entry in fooGroup)
        {
            if (entry.name == pieceName)
            {

                piece = entry;
            }
        }

        GameObject newParent = GameObject.Find("OnBoard");

        piece.transform.parent = newParent.transform;
        piece.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    #endregion

}
