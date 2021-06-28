using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<InputHandler>();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EventsManager.OnLeftMouseInput(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            EventsManager.OnLeftMouseInput(false);
        }
    }

    public Vector3 GetMouseWorldPos()
    {
        Vector3 mousePosOnScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10);// (+10) is to move the mouse away from the camera on z axis otherwise it will not record any movement of the mosue
        return  Camera.main.ScreenToWorldPoint(mousePosOnScreen);
    }
}
