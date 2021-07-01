using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    protected override void Start()
    {
        base.Start();
        EventsManager.onLeftMouseInput += OnLeftMouseInput;
    }

    private void OnLeftMouseInput(bool isItClickDown)
    {
        if (isItClickDown)
        {
            hasToMove = true;
        }
        else
        {
            hasToMove = false;
            firstXMousePos = Mathf.Infinity;
        }
    }

    private void FixedUpdate()
    {
        if (hasToMove)
        {
            MoveForward();
            Rotate();
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        float mousePosOnX = Input.mousePosition.x;

        if (firstXMousePos == Mathf.Infinity)
        {
            firstXMousePos = mousePosOnX;
        }
        else
        {
            secondXMousePos = mousePosOnX;
            transform.Rotate(Vector3.up * (secondXMousePos - firstXMousePos) * rotatingSpeed * Time.deltaTime);
            firstXMousePos = secondXMousePos;
        }
    }

    private void OnDestroy()
    {
        EventsManager.onLeftMouseInput -= OnLeftMouseInput;
    }
}
