using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    protected override void Awake()
    {
        base.Awake();
        EventsManager.onLeftMouseInput += OnLeftMouseInput;
        Physics.gravity *= 2;
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

    public override void OnPrepareNewRace()
    {
        base.OnPrepareNewRace();
    }

    private void OnDestroy()
    {
        EventsManager.onLeftMouseInput -= OnLeftMouseInput;
    }
}
