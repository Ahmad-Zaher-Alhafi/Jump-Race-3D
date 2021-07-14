using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerMovement : Movement
{
    [SerializeField] private float hitForce;

    private Racer racer;
    private bool hasToMoveToNextJumpObject;
    public bool HasToMoveToNextJumpObject
    {
        get => hasToMoveToNextJumpObject;
        set => hasToMoveToNextJumpObject = value;
    }

    protected override void Awake()
    {
        base.Awake();
        racer = GetComponent<Racer>();
    }

    private void FixedUpdate()
    {
        if (racer.IsDead)
        {
            return;
        }

        if (hasToMoveToNextJumpObject)
        {
            MoveToNextJumpObject();
            Rotate();
        }
        else
        {
            JumpObject jumpObject = racer.GetCurrentJumpObject();

            if (jumpObject != null)
            {
                Vector3 objToGoToPos = new Vector3(jumpObject.transform.position.x, transform.position.y, jumpObject.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, objToGoToPos, forwardSpeed / Vector3.Distance(transform.position, jumpObject.transform.position) * Time.deltaTime);
            }
        }
    }

    private void MoveToNextJumpObject()
    {
        JumpObject nextJumpObjectPos = racer.GetNextJumpObject();
        Vector3 objToGoToPos = new Vector3(nextJumpObjectPos.transform.position.x, transform.position.y, nextJumpObjectPos.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, objToGoToPos, forwardSpeed / Vector3.Distance(transform.position, nextJumpObjectPos.transform.position) * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 oldAngle = transform.eulerAngles;
        transform.LookAt(racer.GetNextJumpObject().transform);
        Vector3 newAngle = transform.eulerAngles;
        transform.eulerAngles = oldAngle;

        Vector3 wantedAngle = new Vector3(transform.eulerAngles.x, newAngle.y, transform.eulerAngles.z);

        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, wantedAngle, rotatingSpeed * Time.deltaTime);
    }

    public override void OnPrepareNewRace()
    {
        base.OnPrepareNewRace();
        hasToMoveToNextJumpObject = false;
    }

    public void Die()
    {
        rig.constraints = RigidbodyConstraints.None;
        rig.AddForce(Vector3.one * hitForce, ForceMode.Impulse);
        rig.AddTorque(Vector3.one * hitForce, ForceMode.Impulse);
    }
}
