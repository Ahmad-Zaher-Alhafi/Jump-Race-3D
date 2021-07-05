using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    private Renderer render;
    private RacerMovement racerMovement;
    private JumpObject currentJumpObject;
    private JumpObject jumpObjectToGoTo;
    private int numOfJumpsThatHasDid;
    private CharacterAnimator animator;

    private bool isDead;
    public bool IsDead => isDead;

    private bool hasFinishedTheRace;
    public bool HasFinishedTheRace => hasFinishedTheRace;

    private string racerName;
    public string RacerName
    {
        get => racerName;
        set => racerName = value;
    }

    private Constances.RacerDifficulty racerDifficulty;
    public Constances.RacerDifficulty RacerDifficulty
    {
        get => racerDifficulty;
        set => racerDifficulty = value;
    }

    private int numOfJumpsToMoveToNextObject;
    public int NumOfJumpsToMoveToNextObject
    {
        get => numOfJumpsToMoveToNextObject;
        set => numOfJumpsToMoveToNextObject = value;
    }

    void Awake()
    {
        racerMovement = GetComponent<RacerMovement>();
        render = GetComponent<Renderer>();
        animator = GetComponent<CharacterAnimator>();
        
        numOfJumpsThatHasDid = 0;
        name = racerName;
        animator.playAnimation(Constances.AnimationsTypes.Warmingup);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != Constances.PathJumpObjectTag)
        {
            return;
        }

        if (other.gameObject.layer == Constances.JumpObjectLayerNum)
        {
            JumpObject jumpObject = other.gameObject.GetComponent<JumpObject>();
            if (jumpObject.JumpObjectIndex >= currentJumpObject.JumpObjectIndex)
            {
                currentJumpObject = jumpObject;

                if (currentJumpObject != null)
                {
                    jumpObjectToGoTo = JumpObjectsCreater.Instance.GetNextJumpObject(currentJumpObject.JumpObjectIndex);
                }
            }
            
        }
        else if (other.gameObject.layer == Constances.JumpObjectBaseLayerNum)
        {
            JumpObject jumpObject = other.gameObject.GetComponent<JumpObjectBase>().GetJumpObjectOfThisBase();

            if (jumpObject.JumpObjectIndex >= currentJumpObject.JumpObjectIndex)
            {
                currentJumpObject = jumpObject;
            }
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
                jumpObjectToGoTo = JumpObjectsCreater.Instance.GetNextJumpObject(currentJumpObject.JumpObjectIndex);
            }

            if (other.gameObject.tag == Constances.PathJumpObjectTag)
            {
                racerMovement.Jump(false);
            }
            else
            {
                racerMovement.Jump(true);
            }

            numOfJumpsThatHasDid++;
            racerMovement.IsAbleToJump = false;
            animator.SetAnimatorParameter(true);

            if (!currentJumpObject.IsItLastJumpObject)
            {
                if (numOfJumpsThatHasDid >= numOfJumpsToMoveToNextObject )
                {
                    if (CheckIfHasToDie(racerDifficulty))
                    {
                        Die();
                    }
                    else
                    {
                        numOfJumpsThatHasDid = 0;
                        racerMovement.HasToMoveToNextJumpObject = true;
                    }
                }
            }
            else
            {
                hasFinishedTheRace = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        racerMovement.IsAbleToJump = true;
    }

    public JumpObject GetNextJumpObject()
    {
        return jumpObjectToGoTo;
    }

    public JumpObject GetCurrentJumpObject()
    {
        if (currentJumpObject != null)
        {
            return currentJumpObject;
        }
        else
        {
            return null;
        }
    }

    public void SetCurrentJumpObject(JumpObject jumpObject)
    {
        currentJumpObject = jumpObject;
    }

    public void OnPrepareNewRace()
    {
        numOfJumpsThatHasDid = 0;
        racerMovement.HasToMoveToNextJumpObject = false;
        jumpObjectToGoTo = null;
        animator.playAnimation(Constances.AnimationsTypes.Warmingup);
        isDead = false;
        hasFinishedTheRace = false;
        racerMovement.OnPrepareNewRace();
    }

    public void OnRaceStart()
    {
        racerMovement.OnRaceStart();
        animator.playAnimation(Constances.AnimationsTypes.Warmingup);
    }

    public void Die()
    {
        isDead = true;
        racerMovement.Die();
    }

    private bool CheckIfHasToDie(Constances.RacerDifficulty racerDifficulty)
    {
        int rand;

        switch (racerDifficulty)
        {
            case Constances.RacerDifficulty.Pro:
                return false;
            case Constances.RacerDifficulty.Hard:
                 rand = Random.Range(0, 10);
                return rand == 0 ? true : false;
            case Constances.RacerDifficulty.Normal:
                 rand = Random.Range(0, 7);
                return rand == 0 ? true : false;
            case Constances.RacerDifficulty.Easy:
                 rand = Random.Range(0, 5);
                return rand == 0 ? true : false;
            case Constances.RacerDifficulty.Noob:
                 rand = Random.Range(0, 5);
                return rand == 0 ? true : false;
            default:
                return false;
        }
    }

    public void SetRacerColor(Color color)
    {
        render.material.color = color;
    }
}
