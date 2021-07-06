using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPoint : MonoBehaviour
{
    [SerializeField] private float maxAlpha;
    [SerializeField] private float minAlpha;
    [SerializeField] private float changingAlphaSpeed;
    [SerializeField] private float distanceToStartBlinking;
    [SerializeField] private JumpObject jumpObject;

    private SpriteRenderer spriteRenderer;
    private float alpha;
    private bool hasToBeDeactivated
    {
        get
        {
            if (jumpObject.tag != Constances.PathJumpObjectTag || jumpObject.JumpObjectIndex == 0 || jumpObject.IsItLastJumpObject)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    void Start()
    {
        if (hasToBeDeactivated)
        {
            gameObject.SetActive(false);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        alpha = spriteRenderer.color.a;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) < distanceToStartBlinking)
        {
            Blink();
        }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        }
    }

    private void Blink()
    {
        if (alpha > maxAlpha)
        {
            changingAlphaSpeed *= -1;
            alpha = maxAlpha;
        }
        else if (alpha < minAlpha)
        {
            changingAlphaSpeed *= -1;
            alpha = minAlpha;
        }

        alpha += changingAlphaSpeed * Time.deltaTime;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constances.PlayerLayerNum)
        {
            Player.Instance.OnCenterPointHit();
        }
    }
}
