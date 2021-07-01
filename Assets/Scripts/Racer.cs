using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    [SerializeField] private int numOfJumpsToMoveToNextobject;
    [SerializeField] private Color racerColor;

    private Renderer render;
    private RacerMovement racerMovement;
    private JumpObject currentJumpObject;
    private JumpObject jumpObjectToGoTo;
    private int numOfJumpsThatHasDid;
    private CharacterAnimator animator;

    void Start()
    {
        racerMovement = GetComponent<RacerMovement>();
        render = GetComponent<Renderer>();
        animator = GetComponent<CharacterAnimator>();
        render.material.color = racerColor;
        numOfJumpsThatHasDid = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Constances.JumpObjectLayerNum)
        {
            currentJumpObject = collision.gameObject.GetComponent<JumpObject>();
        }
        else if (collision.gameObject.layer == Constances.JumpObjectBaseLayerNum)
        {
            currentJumpObject = collision.gameObject.GetComponent<JumpObjectBase>().GetJumpObjectOfThisBase();
        }
        else
        {
            return;
        }

        if (racerMovement.IsAbleToJump)
        {
            racerMovement.HasToMoveToNextJumpObject = false;

            if (currentJumpObject != null)
            {
                jumpObjectToGoTo = JumpObjectsCreater.Instance.GetNextJumpObject(currentJumpObject.JumpObjectNumper);
            }

            racerMovement.Jump();
            numOfJumpsThatHasDid++;
            racerMovement.IsAbleToJump = false;
            animator.PlayAnimation(true);

            if (!currentJumpObject.IsItLastJumpObject)
            {
                if (numOfJumpsThatHasDid >= numOfJumpsToMoveToNextobject)
                {
                    numOfJumpsThatHasDid = 0;
                    racerMovement.HasToMoveToNextJumpObject = true;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        racerMovement.IsAbleToJump = true;
    }

    public Vector3 GetNextJumpObjectPos()
    {
        return jumpObjectToGoTo.transform.position;
    }
}
