using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Victory");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            FindObjectOfType<AudioManager>().StopPlaying("Victory");
            SceneManager.LoadScene("MainMenu");
        }
        else
            transform.Translate(Camera.main.transform.up * scrollSpeed * Time.deltaTime);
    }
}
