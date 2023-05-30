using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// USED https://www.youtube.com/watch?v=r6mT8pBn0-k&ab_channel=TheSleepyKoala
// for converting gifs to animations and ScreenToGif for capturing gifs

// Check if a gameobject is active with the .activeInHierarchy property
// and set active with .setActive property

public class Tutorial : MonoBehaviour
{
    // creates variable for 'Main' gameobject menu
    private GameObject tutMenu;
    // variables for 'Intro' gameobject and all children pages
    private GameObject intro, background, howToPlay, explainHelp;
    // variables for 'Pieces' gameobject and all children pages
    private GameObject pieces, queen, ant, beetle, spider, grasshopper;
    // variables for 'Controls' gameobject and all children pages
    private GameObject controls, mouseCtrls, keyCtrls, other;
    // variables for arrowBtn gameobjects
    private GameObject prevBtn, nextBtn;

    void Start()
    {
        //FindObjectOfType<AudioManager>().StopPlaying("MainTheme");
        FindObjectOfType<AudioManager>().Play("Tutorial");

        // assigns gameObjects to their variables
        tutMenu = GameObject.Find("Main");
        intro = GameObject.Find("Intro");
            background = GameObject.Find("Background");
            howToPlay = GameObject.Find("HowToPlay");
            explainHelp = GameObject.Find("ExplainHelp");
        pieces = GameObject.Find("Pieces");
            queen = GameObject.Find("Queen");
            ant = GameObject.Find("Ant");
            beetle = GameObject.Find("Beetle");
            spider = GameObject.Find("Spider");
            grasshopper = GameObject.Find("Grasshopper");
        controls = GameObject.Find("Controls");
            mouseCtrls = GameObject.Find("MouseCtrls");
            keyCtrls = GameObject.Find("KeyCtrls");
            other = GameObject.Find("Other");
        prevBtn = GameObject.Find("PrevBtn");
        nextBtn = GameObject.Find("NextBtn");

        // sets all sub menu and arrowBtn gameobjects to inactive, main to active
        tutMenu.SetActive(true);
        intro.SetActive(false);
        pieces.SetActive(false);
        controls.SetActive(false);
        prevBtn.SetActive(false);
        nextBtn.SetActive(false);
    }

    public void OnClick_goBack()
    {
        FindObjectOfType<AudioManager>().Play("ExitSFX");
        if (tutMenu.activeInHierarchy)
        {
            FindObjectOfType<AudioManager>().StopPlaying("Tutorial");
            SceneManager.LoadScene("MainMenu");
        }
        else //determine if sub menu is active or nothing detected
        {
            if (intro.activeInHierarchy)
                intro.SetActive(false);
            else if (pieces.activeInHierarchy)
                pieces.SetActive(false);
            else if (controls.activeInHierarchy)
                controls.SetActive(false);
            else
            {
                Debug.Log("No active Gameobjects detected");
                return;
            }
            tutMenu.SetActive(true);
            prevBtn.SetActive(false);
            nextBtn.SetActive(false);
        }
    }

    public void OnClick_toIntro()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        //sets intro page up with just background page and nextArrow
        tutMenu.SetActive(false);
        intro.SetActive(true);
        background.SetActive(true);
        howToPlay.SetActive(false);
        explainHelp.SetActive(false);
        nextBtn.SetActive(true);
    }

    public void OnClick_toPieces()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        //sets pieces page up with just queen page and nextArrow
        tutMenu.SetActive(false);
        pieces.SetActive(true);
        queen.SetActive(true);
        spider.SetActive(false);
        beetle.SetActive(false);
        grasshopper.SetActive(false);
        ant.SetActive(false);
        nextBtn.SetActive(true);
    }

    public void OnClick_toControls()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        //sets controls page up with just mouse controls page and nextArrow
        tutMenu.SetActive(false);
        controls.SetActive(true);
        mouseCtrls.SetActive(true);
        keyCtrls.SetActive(false);
        other.SetActive(false);
        nextBtn.SetActive(true);
    }

    public void OnClick_arrowBtn(Button arrowBtn)
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");
        // Checks if the clicked object is nextBtn or prevBtn and assigns to generic arrowBtn
        bool isNextBtn;
        if (arrowBtn.name == "PrevBtn")
            isNextBtn = false;
        else if (arrowBtn.name == "NextBtn") //arrowBtn.name == "prevBtn"
            isNextBtn = true;
        else
        {
            Debug.Log("Error occured in isNextBtn assignment - OnClick_arrowBtn function");
            return;
        }

        if (intro.activeInHierarchy)
        {
            if (background.activeInHierarchy && isNextBtn)
            {
                background.SetActive(false);
                prevBtn.SetActive(true);
                howToPlay.SetActive(true);
            }
            else if (howToPlay.activeInHierarchy)
            {
                howToPlay.SetActive(false);
                if (isNextBtn)
                {
                    nextBtn.SetActive(false);
                    explainHelp.SetActive(true);
                }
                else
                {
                    prevBtn.SetActive(false);
                    background.SetActive(true);
                }
            }
            else if (explainHelp.activeInHierarchy && !isNextBtn)
            {
                explainHelp.SetActive(false);
                nextBtn.SetActive(true);
                howToPlay.SetActive(true);
            }
            else
                Debug.Log("No active Gameobjects detected in intro page");
        }
        else if (pieces.activeInHierarchy)
        {
            if (queen.activeInHierarchy && isNextBtn)
            {
                queen.SetActive(false);
                prevBtn.SetActive(true);
                ant.SetActive(true);
            }
            else if (ant.activeInHierarchy)
            {
                ant.SetActive(false);
                if (isNextBtn)
                    beetle.SetActive(true);
                else
                {
                    prevBtn.SetActive(false);
                    queen.SetActive(true);
                }
            }
            else if (beetle.activeInHierarchy)
            {
                beetle.SetActive(false);
                if (isNextBtn)
                    spider.SetActive(true);
                else
                    ant.SetActive(true);
            }
            else if (spider.activeInHierarchy)
            {
                spider.SetActive(false);
                if (isNextBtn)
                {
                    nextBtn.SetActive(false);
                    grasshopper.SetActive(true);
                }
                else
                    beetle.SetActive(true);
            }
            else if (grasshopper.activeInHierarchy && !isNextBtn)
            {
                grasshopper.SetActive(false);
                nextBtn.SetActive(true);
                spider.SetActive(true);
            }
            else
                Debug.Log("No active Gameobjects detected in pieces page");
        }
        else if (controls.activeInHierarchy)
        {
            if (mouseCtrls.activeInHierarchy && isNextBtn)
            {
                mouseCtrls.SetActive(false);
                prevBtn.SetActive(true);
                keyCtrls.SetActive(true);
            }
            else if (keyCtrls.activeInHierarchy)
            {
                keyCtrls.SetActive(false);
                if (isNextBtn)
                {
                    nextBtn.SetActive(false);
                    other.SetActive(true);
                }
                else
                {
                    prevBtn.SetActive(false);
                    mouseCtrls.SetActive(true);
                }                
            }
            else if (other.activeInHierarchy && !isNextBtn)
            {
                other.SetActive(false);
                nextBtn.SetActive(true);
                keyCtrls.SetActive(true);
            }
            else
                Debug.Log("No active Gameobjects detected in controls page");
        }
        else
            Debug.Log("No active Gameobjects detected");
    }

    public void MouseOn_button()
    {
        FindObjectOfType<AudioManager>().Play("HoverSFX");
    }
}