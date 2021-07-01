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

    public LayerMask JumpAbleObjectsLayers => jumpAbleObjectsLayers;

    private float slowTimeSpeedToAdd;
    private LineRenderer lineRenderer;
    private RaycastHit raycastHit;
    private PlayerMovement playerMovement;
    private Vector3 lineOffsetToAdd;
    private Vector3 linePoint2;
    private CharacterAnimator animator;
    private bool hasToSlowTimeDown;
   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<Player>();
        }
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<CharacterAnimator>();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (IsItHittingJumbAbleObject(collision.gameObject))
        {
            if (playerMovement.IsAbleToJump)
            {
                if (collision.gameObject.tag == Constances.PathJumpObjectTag)
                {
                    playerMovement.Jump(false);
                }
                else
                {
                    playerMovement.Jump(true);
                }

                animator.PlayAnimation(true);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsItHittingJumbAbleObject(collision.gameObject))
        {
            playerMovement.IsAbleToJump = true;
        }
    }

    private void SetLinePositions()
    {
        lineOffsetToAdd = transform.forward * lineOffset.z + transform.up * lineOffset.y + transform.right * lineOffset.x;
        lineRenderer.SetPosition(0, transform.position + lineOffsetToAdd);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, raycastHit.point.y, transform.position.z) + lineOffsetToAdd);
        linePoint2 = lineRenderer.GetPosition(1);
        lineEndSpheerRenderer.transform.position = new Vector3(linePoint2.x, raycastHit.point.y, linePoint2.z);
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
        if (Physics.BoxCast(transform.position + Vector3.up, Vector3.one * .1f, Vector3.down, out raycastHit, Quaternion.identity, Mathf.Infinity/*, 1 << Constances.JumpObjectLayerNum*/))
        {
            if (raycastHit.collider != null && IsItHittingJumbAbleObject(raycastHit.collider.gameObject))
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
        
        gameObject.SetActive(false);
    }
}
