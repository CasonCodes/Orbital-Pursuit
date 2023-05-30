using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedMoves : MonoBehaviour
{
    public GameObject helpSpheres;
    public Material green;
    public Material yellow;

    public void OnTriggerEnter(Collider other)
    {
        
        helpSpheres.gameObject.GetComponent<Renderer>().material = yellow;
    }

    public void OnTriggerExit(Collider other)
    {
        
        helpSpheres.gameObject.GetComponent<Renderer>().material = green;
    }
}
