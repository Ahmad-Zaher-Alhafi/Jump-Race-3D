using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerMovement : Movement
{
    private Racer racer;
    private bool hasToMoveToNextJumpObject;
    public bool HasToMoveToNextJumpObject
    {
        get => hasToMoveToNextJumpObject;
        set => hasToMoveToNextJumpObject = value;
    }

    protected override void Start()
    {
        base.Start();
        racer = GetComponent<Racer>();
    }

    private void FixedUpdate()
    {
        if (hasToMoveToNextJumpObject)
        {
            MoveToNextJumpObject();
            Rotate();
        }
    }

    private void MoveToNextJumpObject()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(racer.GetNextJumpObjectPos().x, transform.position.y, racer.GetNextJumpObjectPos().z), forwardSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 oldAngle = transform.eulerAngles;
        transform.LookAt(racer.GetNextJumpObjectPos());
        Vector3 newAngle = transform.eulerAngles;
        transform.eulerAngles = oldAngle;

        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, newAngle.y, transform.eulerAngles.z), rotatingSpeed * Time.deltaTime);
    }
}
