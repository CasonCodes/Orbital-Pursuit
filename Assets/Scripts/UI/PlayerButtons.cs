using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerButtons : MonoBehaviour
{
   

    public void CreditsButton()
    {
    }

    public void ExitButton()
    {
        Debug.Log("QUIT");
        // Quit Game
        Application.Quit();
    }
}
