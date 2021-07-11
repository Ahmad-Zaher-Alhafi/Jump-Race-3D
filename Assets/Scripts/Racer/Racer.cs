using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : RaceCharacter
{
    private Renderer render;
    private RacerMovement racerMovement;
    private JumpObject jumpObjectToGoTo;
    private int numOfJumpsThatHasDid;


    private bool hasFinishedTheRace;
    public bool HasFinishedTheRace
    {
        get => hasFinishedTheRace;
        set => hasFinishedTheRace = value;
    }

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

    private bool hasWonTheRace;
    public bool HasWonTheRace
    {
        get => hasWonTheRace;
        set => hasWonTheRace = value;
    }
    public Vector3 StartPosOffset
    {
        get => startPosOffset;
    }

    protected override void Awake()
    {
        base.Awake();

        racerMovement = GetComponent<RacerMovement>();
        render = GetComponent<Renderer>();
        numOfJumpsThatHasDid = 0;
        name = racerName;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constances.PlayerLayerNum)
        {
            Player.Instance.OnTriggerEnterWithRacer(transform);
            Die();
        }
    }

    public void OnTriggerEnterWithJumpableObject(JumpObject jumpObject)
    {
        if (jumpObject.IsItLastJumpObject)
        {
            return;
        }

        if (jumpObject.JumpObjectIndex >= currentJumpObject.JumpObjectIndex)
        {
            currentJumpObject = jumpObject;

            rank = currentJumpObject.JumpObjectIndex;
            EventsManager.OnRacerUpdateRank();    

            if (racerMovement.IsAbleToJump)
            {
                racerMovement.HasToMoveToNextJumpObject = false;
                jumpObjectToGoTo = JumpObjectsCreater.Instance.GetNextJumpObject(currentJumpObject.JumpObjectIndex);
                racerMovement.Jump(true);
                numOfJumpsThatHasDid++;
                racerMovement.IsAbleToJump = false;
                animator.playAnimation(Constances.AnimationsTypes.Jump);

                if (numOfJumpsThatHasDid >= numOfJumpsToMoveToNextObject)
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
        }
    }

    public void OnTriggerExitFromJumpAbleObject()
    {
        racerMovement.IsAbleToJump = true;
    }

    public JumpObject GetNextJumpObject()
    {
        return jumpObjectToGoTo;
    }

   

    public override void OnPrepareNewRace(JumpObject startJumpObject)
    {
        base.OnPrepareNewRace(startJumpObject);

        transform.rotation = Quaternion.identity;
        numOfJumpsThatHasDid = 0;
        racerMovement.HasToMoveToNextJumpObject = false;
        jumpObjectToGoTo = null;
        hasFinishedTheRace = false;
        racerMovement.OnPrepareNewRace();
    }

    public void OnRaceStart()
    {
        racerMovement.OnRaceStart();
    }

    public void OnFinishTheRace(bool hasWon)
    {
        hasWonTheRace = hasWon;
        racerMovement.DisableGravity();
        transform.position = new Vector3(transform.position.x, currentJumpObject.transform.position.y + .4f, transform.position.z);

        if (hasWon)
        {
            animator.playAnimation(Constances.AnimationsTypes.Win);
        }
        else
        {
            animator.playAnimation(Constances.AnimationsTypes.Lose);
        }
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
                 rand = Random.Range(0, 15);
                return rand == 0 ? true : false;
            case Constances.RacerDifficulty.Normal:
                 rand = Random.Range(0, 12);
                return rand == 0 ? true : false;
            case Constances.RacerDifficulty.Easy:
                 rand = Random.Range(0, 7);
                return rand == 0 ? true : false;
            case Constances.RacerDifficulty.Noob:
                 rand = Random.Range(0, 7);
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
