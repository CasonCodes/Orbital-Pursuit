using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    int counter = 0;
    public GameObject playersHand;
    public void FlipImage()
    {
        FindObjectOfType<AudioManager>().Play("PressSFX");

        playersHand.SetActive(true);
        counter++;
        if (counter % 2 == 1)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            playersHand.SetActive(true);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            playersHand.SetActive(false);
        }


    }


}
