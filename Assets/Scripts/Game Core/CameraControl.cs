using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IMPORTANT INFORMATION:
// ---------------------------------------------
// CONTROL+M+O collapses all methods


// ---------------------------------------------

public class CameraControl : MonoBehaviour
{
    private Vector3 camMovement;
    public float speedOfZooming = 10f;

    public float camTilt;
    public float camShift;

    public float scroll;
    public Vector3 pos;
    public Vector3 org;
    public Vector3 right;
    public GameObject settingsPanel;

    private void Start()
    {
        camTilt = 90f;
        camShift = 0f;
        org = transform.position;
        pos = org;
    }

    void Update() 
    {
        // WASD controls
        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");
        if (Camera.current != null)
        {
            transform.localPosition += new Vector3(xAxisValue * 0.1f , 0.0f , zAxisValue * 0.1f);            
        }

        // scrolling functionality - if user scrolls mouse wheel, update the height of camera
        // Also caps it at 3f and 50f
        scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            org = transform.position;

            // initialize pos to current camera position if it hasn't been set yet
            if (pos == Vector3.zero) 
            {
                pos = org;
            }
            pos.y += transform.forward.y * scroll * speedOfZooming;
            pos.y = Mathf.Clamp(pos.y, 3f, 50f);
            org.y = pos.y;

            transform.position = org;
        }

        // Camera Tilt Controls
        if (Input.GetMouseButton(1))
        {
            camTilt -= Input.GetAxis("Mouse Y") * 2.0f;
            camShift += Input.GetAxis("Mouse X") * 2.0f;

            camTilt = Mathf.Clamp(camTilt, 20f, 90f);
            camShift = Mathf.Clamp(camShift, -20f, 20f);

            right = transform.eulerAngles;
            right.x = camTilt;
            right.y = camShift;
            right.z = 0;

            transform.eulerAngles = right;
        }       
    }

    public void ResetCamera()
    {
        if (GameState.fightingAI)
        {
            transform.position = org;
            transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            settingsPanel.SetActive(false);
        }
        else
        {
            try
            {
                GameObject cam = GameObject.Find("HostCamera");
                cam.transform.position = new Vector3(0, 7, 0);
                cam.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                settingsPanel.SetActive(false);
            }
            catch
            {
                GameObject cam = GameObject.Find("ClientCamera");
                cam.transform.position = new Vector3(0, 7, 0);
                cam.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                settingsPanel.SetActive(false);
            }
        }
    }

}
