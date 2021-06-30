using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private Material validGroundLineMaterial;
    [SerializeField] private Material InvalidGroundLineMaterial;
    [SerializeField] private Material validGroundLineEndMaterial;
    [SerializeField] private Material InvalidGroundLineEndMaterial;
    [SerializeField] private Renderer lineEndRenderer;

    private LineRenderer lineRenderer;
    private RaycastHit raycastHit;
    private PlayerMovement playerMovement;

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
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position + Vector3.up * .5f);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, raycastHit.point.y, transform.position.z));
        lineEndRenderer.transform.position = new Vector3(lineEndRenderer.transform.position.x, raycastHit.point.y, lineEndRenderer.transform.position.z);

        if (CheckForJumpObject())
        {
            lineRenderer.material = validGroundLineMaterial;
            lineEndRenderer.material = validGroundLineEndMaterial;
        }
        else
        {
            lineRenderer.material = InvalidGroundLineMaterial;
            lineEndRenderer.material = InvalidGroundLineEndMaterial;
        }
    }

    private bool CheckForJumpObject()
    {
        if (Physics.BoxCast(transform.position + Vector3.up, Vector3.one * .1f, Vector3.down, out raycastHit, Quaternion.identity, Mathf.Infinity/*, 1 << Constances.JumpObjectLayerNum*/))
        {
            if (raycastHit.collider != null && raycastHit.collider.gameObject.layer == Constances.JumpObjectLayerNum)
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
