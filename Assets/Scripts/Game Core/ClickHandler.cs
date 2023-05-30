using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IMPORTANT INFORMATION:
// ---------------------------------------------
// CONTROL+M+O collapses all methods


// ---------------------------------------------

public class ClickHandler : MonoBehaviour
{
    Vector3 mousePosition;
    float constantYValue;

    // obtain mouse position using camera view
    private Vector3 GetMousePosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    // obtain position of mouse by calculating the offset of the original mouse position and the new mouse position
    private void OnMouseDown()
    {
        //Debug.Log("Start Drag");
        mousePosition = Input.mousePosition - GetMousePosition();
    }

    // while dragging, update the position of the selected object
    private void OnMouseDrag()
    {
        // Get the current mouse position in screen coordinates
        Vector3 mousePosScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);

        // Convert the mouse position from screen coordinates to world coordinates
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);

        // Set the y position of the object to a constant value to prevent it from moving up or down
        mousePosWorld.y = constantYValue;

        // Set the position of the object to the mouse position in world coordinates
        transform.position = mousePosWorld;
    }

    private void OnMouseUp()
    {
        //Debug.Log("End Drag");
    }
}