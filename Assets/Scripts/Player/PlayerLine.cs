using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLine : MonoBehaviour
{
    [SerializeField] private Material validGroundLineMaterial;
    [SerializeField] private Material invalidGroundLineMaterial;
    [SerializeField] private Material validGroundLineEndSpheerMaterial;
    [SerializeField] private Material invalidGroundLineSpheerEndMaterial;
    [SerializeField] private Renderer lineEndSpheerRenderer;
    [SerializeField] private LayerMask jumpAbleObjectsLayers;
    [SerializeField] private Vector3 lineOffset;

    private LineRenderer lineRenderer;
    private Vector3 lineOffsetToAdd;
    private Vector3 linePoint2;
    private RaycastHit raycastHit;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineEndSpheerRenderer.gameObject.SetActive(false);
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

    public void OnPrepareNewRace()
    {
        lineEndSpheerRenderer.gameObject.SetActive(false);
        lineRenderer.enabled = false;
    }

    public void OnRaceStart()
    {
        lineRenderer.enabled = true;
        lineEndSpheerRenderer.gameObject.SetActive(true);
    }
}
