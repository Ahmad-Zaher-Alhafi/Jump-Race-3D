using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private Material validGroundLineMaterial;
    [SerializeField] private Material invalidGroundLineMaterial;
    [SerializeField] private Material validGroundLineEndSpheerMaterial;
    [SerializeField] private Material invalidGroundLineSpheerEndMaterial;
    [SerializeField] private Renderer lineEndSpheerRenderer;
    [SerializeField] private Vector3 lineOffset;
    [SerializeField] private LayerMask jumpAbleObjectsLayers;
    [SerializeField] private float slowTimeSpeed;
    [SerializeField] private MainCanves mainCanves;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private Vector3 startPosOffset;

    private float slowTimeSpeedToAdd;
    private LineRenderer lineRenderer;
    private RaycastHit raycastHit;
    private PlayerMovement playerMovement;
    private Vector3 lineOffsetToAdd;
    private Vector3 linePoint2;
    private CharacterAnimator animator;
    private bool hasToSlowTimeDown;
    private JumpObject currentJumpObject;
    private bool isDead;
    public bool IsDead => isDead;
    private bool hasWonTheRace;
    public bool HasWonTheRace
    {
        get => hasWonTheRace;
        set => hasWonTheRace = value;
    }

    private int playerRank;
    public int PlayerRank
    {
        get => playerRank;
        set => playerRank = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<Player>();
        }

        lineRenderer = GetComponent<LineRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<CharacterAnimator>();
        animator.playAnimation(Constances.AnimationsTypes.Warmingup);

        lineEndSpheerRenderer.gameObject.SetActive(false);
        lineRenderer.enabled = false;
    }

    void Update()
    {
        SetLinePositions();

        if (CheckForJumpObject())
        {
            lineRenderer.material = validGroundLineMaterial;
            lineEndSpheerRenderer.material = validGroundLineEndSpheerMaterial;
        }
        else
        {
            lineRenderer.material = invalidGroundLineMaterial;
            lineEndSpheerRenderer.material = invalidGroundLineSpheerEndMaterial;
        }

        if (hasToSlowTimeDown)
        {
            SlowTimeDown();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsItHittingJumbAbleObject(other.gameObject))
        {
            if (playerMovement.IsAbleToJump)
            {
                if (other.gameObject.layer == Constances.JumpObjectLayerNum)
                {
                    currentJumpObject = other.gameObject.GetComponent<JumpObject>();
                }
                else if (other.gameObject.layer == Constances.JumpObjectBaseLayerNum)
                {
                    currentJumpObject = other.gameObject.GetComponent<JumpObjectBase>().GetJumpObjectOfThisBase();
                }

                if (currentJumpObject.IsItLastJumpObject)
                {
                    return;
                }

                playerRank = currentJumpObject.JumpObjectIndex;
                currentJumpObject.OrderToRotate();
                mainCanves.SetSliderValue(currentJumpObject.JumpObjectIndex);

                if (other.gameObject.tag == Constances.PathJumpObjectTag)
                {
                    playerMovement.Jump(false);
                }
                else
                {
                    playerMovement.Jump(true);
                }

                animator.SetAnimatorParameter(true);
            }
        }
        else if (other.gameObject.layer == Constances.RacerLayerNum)
        {
            hitParticles.transform.position = other.transform.position;
            hitParticles.Play();
            other.GetComponent<Racer>().Die();
            mainCanves.SetStateTxt(Constances.StateTxtWords.Killer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsItHittingJumbAbleObject(other.gameObject))
        {
            playerMovement.IsAbleToJump = true;
        }
    }

    private void SetLinePositions()
    {
        lineOffsetToAdd = transform.forward * lineOffset.z + transform.up * lineOffset.y + transform.right * lineOffset.x;
        lineRenderer.SetPosition(0, transform.position + lineOffsetToAdd);

        if (raycastHit.collider != null)
        {
            lineRenderer.SetPosition(1, new Vector3(transform.position.x, raycastHit.point.y, transform.position.z) + lineOffsetToAdd);
            linePoint2 = lineRenderer.GetPosition(1);
            lineEndSpheerRenderer.transform.position = new Vector3(linePoint2.x, raycastHit.point.y, linePoint2.z);
        }
        else
        {
            lineRenderer.SetPosition(1, new Vector3(transform.position.x, -100, transform.position.z) + lineOffsetToAdd);
            linePoint2 = lineRenderer.GetPosition(1);
            lineEndSpheerRenderer.transform.position = new Vector3(linePoint2.x, -100, linePoint2.z);
        }
    }

    private bool IsItHittingJumbAbleObject(GameObject objectThatCollidedWith)
    {
        if (objectThatCollidedWith.layer == Constances.JumpObjectLayerNum || objectThatCollidedWith.layer == Constances.JumpObjectBaseLayerNum)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckForJumpObject()
    {
        if (Physics.BoxCast(transform.position + Vector3.up, Vector3.one * .1f, Vector3.down, out raycastHit, Quaternion.identity, Mathf.Infinity, jumpAbleObjectsLayers))
        {
            if (raycastHit.collider != null)
            {
                return true;
            }
            else
            {
                
                return false;
            }
        }
        else
        {
            return false;
        }
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
        mainCanves.SetStateTxt(Constances.StateTxtWords.Prefect);
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
        gameManager.FinishRace(false);
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(.05f);
        gameObject.SetActive(false);
    }

    public void OnPrepareNewRace(JumpObject startJumpObject)
    {
        gameObject.SetActive(true);
        transform.position = startJumpObject.transform.position + startPosOffset;
        lineEndSpheerRenderer.gameObject.SetActive(false);
        lineRenderer.enabled = false;
        isDead = false;
        playerMovement.OnPrepareNewRace();
        animator.playAnimation(Constances.AnimationsTypes.Warmingup);
    }

    public void OnRaceStart()
    {
        lineRenderer.enabled = true;
        lineEndSpheerRenderer.gameObject.SetActive(true);
        playerMovement.OnRaceStart();
    }

    public void OnFinishTheRace(bool hasWon)
    {
        hasWonTheRace = hasWon;
        playerMovement.DisableGravity();

        if (hasWon)
        {
            animator.playAnimation(Constances.AnimationsTypes.Win);
        }
        else
        {
            animator.playAnimation(Constances.AnimationsTypes.Lose);
        }
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
}