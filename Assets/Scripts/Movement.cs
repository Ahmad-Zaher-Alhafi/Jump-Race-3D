using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] protected float forwardSpeed;
    [SerializeField] protected float rotatingSpeed;
    [SerializeField] protected float jumpforce;
    [SerializeField] protected float lowestYPosToLose;
    [SerializeField] protected float speedUpIncreaseValue;
    [SerializeField] protected float speedUpTime;


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

    protected virtual void Start()
    {
        rig = GetComponent<Rigidbody>();
        firstXMousePos = Mathf.Infinity;
        initialForwardSpeed = forwardSpeed;
        speedUpPeriod = new WaitForSeconds(speedUpTime);
    }

    public void Jump(bool isItLongJump)
    {
        isAbleToJump = false;
        rig.velocity = Vector3.zero;

        if (isItLongJump)
        {
            rig.AddForce(Vector3.up * jumpforce * 2, ForceMode.Impulse);
        }
        else
        {
            rig.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }
                    
    }

    public void SpeedUp()
    {
        if (backToNormalSpeedCoroutine != null)
        {
            StopCoroutine(backToNormalSpeedCoroutine);
        }

        if (forwardSpeed != initialForwardSpeed + speedUpIncreaseValue)
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
}
