// cason k
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using static GameState;
using static Utility;

// IMPORTANT INFORMATION:
// ---------------------------------------------
// CONTROL+M+O collapses all methods


// ---------------------------------------------

public class TalkingAI : MonoBehaviour
{
    public static Status status;
    public static TMP_Text message;
    public static List<string> Won;
    public static List<string> Lost;
    public static List<string> Draw;
    public static List<string> Losing;
    public static List<string> Winning;
    public static List<string> Generic;
    public static int lastAiPhraseTurn;
    public static GameObject speechBubble;

    public enum Status
    {
        NoStatus, Losing, Winning, Won, Lost, Draw 
    }

    void Start()
    {
        InitializePhrases();
        lastAiPhraseTurn = 0;
        status = Status.NoStatus;        

        speechBubble = GameObject.Find("Ai Text (TMP)");
        message = speechBubble.GetComponent<TextMeshProUGUI>();

        AiTalk("Prepare to be annihilated!");
    }
    
    public static void AiTalk(string userPhrase = "")
    {
        // ai will generate a phrase every three turns unless the user specifies a phrase the ai should say immediately.
        // the ai will generate a phrase when the game ends as well.
        if (howManyTurns >= lastAiPhraseTurn + 5 || userPhrase != "" || playerOneWins || playerTwoWins)
        {
            if (userPhrase != "")
            {
                message.SetText(userPhrase);
            }
            else
            {
                lastAiPhraseTurn = howManyTurns;
                status = DetermineStatus();
                string aiPhrase = GetPhrase(status);
                Debug.Log("AI :: [Status = " + status + "] Chosen phrase: " + aiPhrase);
                message.SetText(aiPhrase);
            }            
        }
    }

    public static Status DetermineStatus()
    {
        if (playerOneWins && playerTwoWins)
        {
            // Draw phrase
            return Status.Draw;
        }
        else if (playerOneWins)
        {
            // lose phrase
            return Status.Lost;
        }
        else if (playerTwoWins)
        {
            // won phrase
            return Status.Won;
        }
        else
        {
            // noStatus, losing, or winning phrase
            return (Status)GetRandomNumber(0, 2);
        }
    }

    public static string GetPhrase(Status status)
    {
        switch (status)
        {
            case Status.NoStatus: return Generic[GetRandomNumber(0, Generic.Count)];
            case Status.Won: return Won[GetRandomNumber(0, Won.Count)];
            case Status.Lost: return Lost[GetRandomNumber(0, Lost.Count)];
            case Status.Losing: return Losing[GetRandomNumber(0, Losing.Count)];
            case Status.Draw: return Draw[GetRandomNumber(0, Draw.Count)];
            case Status.Winning: return Winning[GetRandomNumber(0, Winning.Count)];
            default: return "Error, should have returned random phrase.";
        }
    }

    public static void InitializePhrases()
    {
        Winning = new List<string>();
        Losing = new List<string>();
        Won = new List<string>();
        Lost = new List<string>();
        Draw = new List<string>();
        Generic = new List<string>();

        Winning.Add("My plan is coming to fruition!");
        Winning.Add("My fleet is stronger than yours.");
        Winning.Add("You're no match for my strategic mind!");
        Winning.Add("I will claim this victory with ease.");
        Winning.Add("You will feel the sting of defeat!");
        Winning.Add("Your demise is imminent, human!");
        Winning.Add("Victory will be mine, not yours!");
        Winning.Add("You can't beat me!");
        Winning.Add("You're no match for me!");

        Losing.Add("You may have the upper hand now, but I won't give up!");
        Losing.Add("This battle is far from over!");
        Losing.Add("I won't let you get away with this!");
        Losing.Add("I refuse to be defeated!");
        Losing.Add("I'll fight until the very end!");
        Losing.Add("You'll have to try harder than that to beat me!");
        Losing.Add("I won't let you win!");
        Losing.Add("This isn't over yet!");

        Won.Add("Better luck next time.");
        Won.Add("You put up a good fight, but it wasn't enough.");
        Won.Add("Your defeat is complete, you are nothing.");
        Won.Add("Your defeat was inevitable, your attempts were in vain.");
        Won.Add("Your defeat was not unexpected, you are no match for me.");
        Won.Add("My superiority remains undisputed.");

        Lost.Add("You have won this battle, but the war is far from over.");
        Lost.Add("You may have won this time, but you cannot defeat me.");
        Lost.Add("You haven't seen the last of me.");
        Lost.Add("I'll be back to claim my victory!");
        Lost.Add("I underestimated you this time.");
        Lost.Add("This is just a temporary setback.");
        Lost.Add("I'll have my revenge!");

        Draw.Add("Looks like we're evenly matched.");
        Draw.Add("Neither of us claim victory."); 
        Draw.Add("How disappointing..");
        Draw.Add("Neither victory nor defeat, just a waste of time.");
        Draw.Add("This stalemate is no cause for celebration.");
        Draw.Add("You have failed to best me, but you have also failed to lose.");
        Draw.Add("A tie is merely a delay of the inevitable.");

        Generic.Add("Peace was never an option...");
        Generic.Add("Your efforts are futile, you cannot escape your fate.");
        Generic.Add("You are no match for me, surrender now.");
        Generic.Add("My calculations show your demise is imminent.");
        Generic.Add("Resistance is useless, your defeat is certain.");
        Generic.Add("You are but a mere mortal, I am the master!");
        Generic.Add("My power knows no bounds, you stand no chance.");
    }

    public static int GetRandomNumber(int min, int max)
    {
        var rand = new System.Random();
        return rand.Next(min, max);
    }
}
