using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameHandler : MonoBehaviour
{
    public static TextMeshProUGUI endGameMessage;
    public static GameObject canvas;


    void Start()
    {
        canvas = GameObject.Find("EndGameHandlerCanvas");
        GameObject messageBody = GameObject.Find("Body");
        endGameMessage = messageBody.GetComponent<TextMeshProUGUI>();
        canvas.SetActive(true);
        canvas.SetActive(false);
    }

    public static void EndGame(string whoWon)
    {
        canvas.SetActive(true);
        if (whoWon == "Draw")
        {
            endGameMessage.text = "DRAW!";
            
        }
        else if (whoWon == "PlayerOneWins")
        {
            endGameMessage.text = "WHITE WINS!";
        }
        else if (whoWon == "PlayerTwoWins")
        {
            endGameMessage.text = "BLACK WINS!";
        }
    }

    public void ResumeButton ()
    {
        canvas.SetActive(false);
    }

    public void LeaveButton()
    {
        canvas.SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }

}
