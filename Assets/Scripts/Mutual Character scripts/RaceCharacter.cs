using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCharacter : MonoBehaviour
{
    [SerializeField] protected Vector3 startPosOffset;

    protected CharacterAnimator animator;
    protected JumpObject currentJumpObject;

    protected bool isDead;
    public bool IsDead => isDead;

    protected int rank;
    public int Rank
    {
        get => rank;
        set => rank = value;
    }

    protected virtual void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public virtual void OnPrepareNewRace(JumpObject startJumpObject)
    {
        currentJumpObject = startJumpObject;
        transform.position = startJumpObject.transform.position + startPosOffset;
        isDead = false;
        animator.playAnimation(Constances.AnimationsTypes.Warmingup);
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
