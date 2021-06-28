using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float lowestYPosToLose;

    private Rigidbody rig;
    private bool hasToMove;
    private float firstXMousePos;
    private float secondXMousePos;


    void Start()
    {
        EventsManager.onLeftMouseInput += OnLeftMouseInput;

        rig = GetComponent<Rigidbody>();
        firstXMousePos = Mathf.Infinity;
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

    public void Jump()
    {
        rig.velocity = Vector3.zero;
        rig.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Jump();
    }

    private void OnDestroy()
    {
        EventsManager.onLeftMouseInput -= OnLeftMouseInput;
    }
}
