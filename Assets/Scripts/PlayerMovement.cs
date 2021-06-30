using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float lowestYPosToLose;
    [SerializeField] private float speedUpIncreaseValue;
    [SerializeField] private float speedUpTime;
    [SerializeField] private Animation[] animations;

    private Rigidbody rig;
    private bool hasToMove;
    private float firstXMousePos;
    private float secondXMousePos;
    private float initialForwardSpeed;
    private WaitForSeconds speedUpPeriod;
    private Coroutine backToNormalSpeedCoroutine;

    void Start()
    {
        EventsManager.onLeftMouseInput += OnLeftMouseInput;

        rig = GetComponent<Rigidbody>();
        firstXMousePos = Mathf.Infinity;
        initialForwardSpeed = forwardSpeed;
        speedUpPeriod = new WaitForSeconds(speedUpTime);
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
        animator.SetBool("hasToJump", true);
        animator.SetInteger("AnimationNum", Random.Range(0, 3));
    }

    public void Fall()
    {
        animator.SetBool("hasToJump", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Jump();
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

    private void OnDestroy()
    {
        EventsManager.onLeftMouseInput -= OnLeftMouseInput;
    }
}
