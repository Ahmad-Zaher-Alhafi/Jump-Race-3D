using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] protected float forwardSpeed;
    [SerializeField] protected float rotatingSpeed;
    [SerializeField] protected float lowestYPosToLose;
    [SerializeField] protected float speedUpIncreaseValue;
    [SerializeField] protected float speedUpTime;
    [SerializeField] private float normalJumpForce;
    [SerializeField] private float longJumpForce;

    protected Rigidbody rig;
    protected bool hasToMove;
    protected float firstXMousePos;
    protected float secondXMousePos;
    protected float initialForwardSpeed;
    protected WaitForSeconds speedUpPeriod;
    protected Coroutine backToNormalSpeedCoroutine;
    protected bool isAbleToJump = true;//to prevent the player from jumping stwice accedently by one collide with the jump object
    public bool IsAbleToJump
    {
        get => isAbleToJump;
        set => isAbleToJump = value;
    }

    private bool hasSpeededUp
    {
        get => forwardSpeed != initialForwardSpeed + speedUpIncreaseValue;
    }

    protected virtual void Awake()
    {
        rig = GetComponent<Rigidbody>();
        firstXMousePos = Mathf.Infinity;
        initialForwardSpeed = forwardSpeed;
        speedUpPeriod = new WaitForSeconds(speedUpTime);
        DisableGravity();
    }

    public void Jump(bool isItNormalJump)
    {
        isAbleToJump = false;
        rig.velocity = Vector3.zero;

        if (isItNormalJump)
        {
            rig.AddForce(Vector3.up * normalJumpForce, ForceMode.Impulse);
        }
        else
        {
            rig.AddForce(Vector3.up * longJumpForce, ForceMode.Impulse);
        }
    }

    public void SpeedUp()
    {
        if (backToNormalSpeedCoroutine != null)
        {
            StopCoroutine(backToNormalSpeedCoroutine);
        }

        if (!hasSpeededUp)
        {
            forwardSpeed += speedUpIncreaseValue;
        }

        backToNormalSpeedCoroutine = StartCoroutine(GetBackToNormalSpeed());
    }

    private IEnumerator GetBackToNormalSpeed()
    {
        yield return speedUpPeriod;
        forwardSpeed = initialForwardSpeed;
    }

    public void DisableGravity()
    {
        rig.constraints = RigidbodyConstraints.FreezeAll;
        rig.useGravity = false;
        rig.velocity = Vector3.zero;
    }

    public virtual void OnPrepareNewRace()
    {
        DisableGravity();
    }

    public void OnRaceStart()
    {
        rig.constraints = ~RigidbodyConstraints.FreezePositionY;
        rig.useGravity = true;
    }
}
