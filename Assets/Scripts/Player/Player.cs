using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : RaceCharacter
{
    public static Player Instance;

    [SerializeField] private float slowTimeSpeed;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private Collider solidCollider;

    private PlayerLine playerLine;
    private PlayerMovement playerMovement;
    private float slowTimeSpeedToAdd;
    private bool hasToSlowTimeDown;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = GetComponent<Player>();
        }

        playerLine = GetComponent<PlayerLine>();
        playerMovement = GetComponent<PlayerMovement>();
       
    }

    void Update()
    {
        if (hasToSlowTimeDown)
        {
            SlowTimeDown();
        }
    }

    public void OnTriggerEnterWithJumpableObject(bool isItJumpObjectBase, JumpObject jumpObject, bool isItPathJumpObject)
    {
        if (!isItJumpObjectBase)
        {
            solidCollider.enabled = false;
        }
        

        if (playerMovement.IsAbleToJump)
        {
            currentJumpObject = jumpObject;
            EventsManager.OnRacerUpdateRank();
            MainCanves.Instance.SetSliderValue(currentJumpObject.JumpObjectIndex);

            if (!isItJumpObjectBase && isItPathJumpObject && !currentJumpObject.IsItLastJumpObject)
            {
                MainCanves.Instance.SetStateTxt(Constances.StateTxtWords.Good);
            }

            if (!currentJumpObject.IsItLastJumpObject)
            {
                rank = currentJumpObject.JumpObjectIndex;
                playerMovement.Jump(isItPathJumpObject);
                animator.playAnimation(Constances.AnimationsTypes.Jump);
            }
        }
    }

    public void OnTriggerExitFromJumpAbleObject()
    {
        solidCollider.enabled = true;
        playerMovement.IsAbleToJump = true;
    }

    public void OnTriggerEnterWithRacer(Transform racerTransform)
    {
        hitParticles.transform.position = racerTransform.position;
        hitParticles.Play();
        MainCanves.Instance.SetStateTxt(Constances.StateTxtWords.Killer);
    }

    /// <summary>
    /// Will be called when the player hit the center point of the jump object
    /// </summary>
    public void OnCenterPointHit()
    {
        hasToSlowTimeDown = true;
        Time.timeScale = .3f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        playerMovement.SpeedUp();
        MainCanves.Instance.SetStateTxt(Constances.StateTxtWords.Prefect);
    }

    private void SlowTimeDown()
    {
        slowTimeSpeedToAdd += slowTimeSpeed * Time.deltaTime;

        if (Time.timeScale < 1)
        {
            Time.timeScale += slowTimeSpeedToAdd * Time.deltaTime;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;//needed for smooth low motion otherwise it's gonna lag
        }
        else
        {
            hasToSlowTimeDown = false;
            slowTimeSpeedToAdd = 0;
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }

    public void Die()
    {
        isDead = true;
        GameManager.Instance.FinishRace(false);
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(.05f);
        gameObject.SetActive(false);
    }

    public override void OnPrepareNewRace(JumpObject startJumpObject)
    {
        gameObject.SetActive(true);

        base.OnPrepareNewRace(startJumpObject);
        
        playerLine.OnPrepareNewRace();
        
        playerMovement.OnPrepareNewRace();
        
        CorrectPlayerRotation();
    }

    /// <summary>
    /// Let the player looks towards other racers at the begining of the race
    /// </summary>
    private void CorrectPlayerRotation()
    {
        Vector3 initialAngle = transform.eulerAngles;
        transform.LookAt(RacersCreater.Instance.racers[0].transform.position);
        transform.eulerAngles = new Vector3(initialAngle.x, transform.eulerAngles.y, initialAngle.z);
    }

    public void OnRaceStart()
    {
        playerLine.OnRaceStart();
        playerMovement.OnRaceStart();
    }

    public void OnFinishTheRace(bool hasWon)
    {
        playerMovement.DisableGravity();

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
}