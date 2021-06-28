using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private Material greenLineMaterial;
    [SerializeField] private Material redLineMaterial;

    private LineRenderer lineRenderer;
    private RaycastHit raycastHit;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z));

        if (CheckForJumpObject())
        {
            lineRenderer.material = greenLineMaterial;
        }
        else
        {
            lineRenderer.material = redLineMaterial;
        }
    }

    private bool CheckForJumpObject()
    {
        if (Physics.BoxCast(transform.position, Vector3.one * .5f, Vector3.down, out raycastHit, Quaternion.identity, Mathf.Infinity, 1 << Constances.JumpObjectLayerNum))
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
}
