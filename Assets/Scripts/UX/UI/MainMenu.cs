using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//https://sharpcoderblog.com/blog/unity-3d-create-main-menu-with-ui-canvas
//Used this tutorial to set up main menu

public class MainMenu : MonoBehaviour
{
    // Creates global variables to use in other functions
    GameObject mainMenu;
    GameObject playMenu;
    GameObject storyMenu;
    GameObject settingsMenu;
    public Toggle myToggle;

    void Start ()
    {
        FindObjectOfType<AudioManager>().Play("GameState");

        // Finds grouped buttons in scene and deactivates all but MainMenu buttons
        mainMenu = GameObject.Find("MainMenu");
        playMenu = GameObject.Find("PlayMenu");
        storyMenu = GameObject.Find("StoryMenu");
        settingsMenu = GameObject.Find("Settings Panel");
        //myToggle = GetComponent<Toggle>();
        playMenu.SetActive(false);
        storyMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    void Update()
    {
        if (myToggle.isOn)
        {
            GameState.whoGoesFirst = "Black";
        }
        else
        {
            GameState.whoGoesFirst = "White";
        }
    }

    /**********************************************************************************************************************
     ON CLICK EVENTS - MAIN MENU
    ***********************************************************************************************************************/
    //PLAY BUTTON
    public void OnClick_toPlayMenu()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    //TUTORIAL BUTTON
    public void OnClick_toTutorial()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        settingsMenu.SetActive(false);

        SceneManager.LoadScene("Tutorial");
    }

    //OPTIONS BUTTON
    public void OnClick_options()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");

        settingsMenu.SetActive(true);

    }

    //CREDITS BUTTON
    public void OnClick_credits()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        settingsMenu.SetActive(false);

        SceneManager.LoadScene("Credits");
    }

    //EXIT BUTTON
    public void OnClick_exit()
    {
        FindObjectOfType<AudioManager>().Play("ExitSFX");

        Debug.Log("QUIT");
        // Quit Game
        Application.Quit();
    }

    /**********************************************************************************************************************
     PLAY MENU
    ***********************************************************************************************************************/
    public void OnClick_toStory()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        playMenu.SetActive(false);
        storyMenu.SetActive(true);
        /*
        GameState.fightingAI = true;
        GameState.sendOverNetwork = false;
        SceneManager.LoadScene("GameplayScene");
        */
    }

    // Andres Gomez - Made in order to access and test host/client connection.
    public void OnClick_toOnline()
    {
        
        FindObjectOfType<AudioManager>().Play("PressSFX");
        GameState.fightingAI = false;
        GameState.sendOverNetwork = true;
        
        SceneManager.LoadScene("Rooms");
    }

    public void OnClick_backToMain()
    {
        FindObjectOfType<AudioManager>().Play("ExitSFX");

        // Sets everything back to main menu
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
    }

    /**********************************************************************************************************************
     STORY MENU
    ***********************************************************************************************************************/
    public void OnClick_toEasy()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        GameState.fightingAI = true;
        AI.difficulty = AI.Difficulty.Easy;
        GameState.sendOverNetwork = false;
        SceneManager.LoadScene("GameplayScene");
    }

    // Andres Gomez - Made in order to access and test host/client connection.
    public void OnClick_toMedium()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        GameState.fightingAI = true;
        AI.difficulty = AI.Difficulty.Medium;
        GameState.sendOverNetwork = false;
        SceneManager.LoadScene("GameplayScene");
    }

    public void OnClick_toHard()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        GameState.fightingAI = true;
        AI.difficulty = AI.Difficulty.Hard;
        GameState.sendOverNetwork = false;
        SceneManager.LoadScene("GameplayScene");
    }

    public void OnClick_backToPlay()
    {
        FindObjectOfType<AudioManager>().Play("ExitSFX");

        // Sets everything back to play menu
        storyMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    /**********************************************************************************************************************
     MOUSE ON (HOVER) EVENT
    ***********************************************************************************************************************/
    public void MouseOn_button()
    {
        FindObjectOfType<AudioManager>().Play("HoverSFX");
    }



    /**********************************************************************************************************************
     * FOUND ANSWER AT - https://answers.unity.com/questions/36135/how-to-call-a-function-from-a-script-in-another-sc.html
    void Awake()
    {
        // MAYBE NEED TO NOT DESTROY THE SCENE??
        DontDestroyOnLoad(this);
    }
    CORRESPONDS TO CreateRoomMenu
    ***********************************************************************************************************************/
}
