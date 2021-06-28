using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float lowestYPosToLose;
    [SerializeField] private Animator animator;

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
        //if (PlayerMeshCharacter.Instance.IsPlayerAlive && transform.position.y <= lowestYPosToLose)
        //{
        //    PlayerMeshCharacter.Instance.Die();
        //}

        //if (!PlayerMeshCharacter.Instance.IsPlayerAlive)
        //{
        //    rig.useGravity = false;
        //    return;
        //}

        //rig.useGravity = true;

        if (hasToMove)
        {
            //animator.SetBool(Constances.PlayerHasToRunParameter, true);
            MoveForward();
            Rotate();
        }
        else
        {
            //animator.SetBool(Constances.PlayerHasToRunParameter, false);
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);//move forward
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
        }

        transform.Rotate(Vector3.up * (secondXMousePos - firstXMousePos) * rotatingSpeed * Time.deltaTime);
        firstXMousePos = secondXMousePos;
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
