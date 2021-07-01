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
    public LayerMask JumpAbleObjectsLayers => jumpAbleObjectsLayers;

    private LineRenderer lineRenderer;
    private RaycastHit raycastHit;
    private PlayerMovement playerMovement;
    private Vector3 lineOffsetToAdd;
    private Vector3 linePoint2;
    private CharacterAnimator animator;
    private bool isHittingJumbAbleObject
    {
        get
        {
            if (raycastHit.collider.gameObject.layer == Constances.JumpObjectLayerNum || raycastHit.collider.gameObject.layer == Constances.JumpObjectBaseLayerNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

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
        lineOffsetToAdd = transform.forward * lineOffset.z + transform.up * lineOffset.y + transform.right * lineOffset.x;
        lineRenderer.SetPosition(0, transform.position + lineOffsetToAdd);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, raycastHit.point.y, transform.position.z) + lineOffsetToAdd);
        linePoint2 = lineRenderer.GetPosition(1);
        lineEndSpheerRenderer.transform.position = new Vector3(linePoint2.x, raycastHit.point.y, linePoint2.z);

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHittingJumbAbleObject)
        {
            if (playerMovement.IsAbleToJump)
            {
                playerMovement.Jump();
                animator.PlayAnimation(true);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        playerMovement.IsAbleToJump = true;
    }

    private bool CheckForJumpObject()
    {
        if (Physics.BoxCast(transform.position + Vector3.up, Vector3.one * .1f, Vector3.down, out raycastHit, Quaternion.identity, Mathf.Infinity/*, 1 << Constances.JumpObjectLayerNum*/))
        {
            if (raycastHit.collider != null && isHittingJumbAbleObject)
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
    /// To speed up when the player hit the center point of the jump object
    /// </summary>
    public void SpeedUp()
    {
        playerMovement.SpeedUp();
    }
}
