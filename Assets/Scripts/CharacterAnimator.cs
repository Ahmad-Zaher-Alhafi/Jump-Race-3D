using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(bool hasToJump)
    {
        if (hasToJump)
        {
            animator.SetBool(Constances.HasToJumpParameter, true);
            animator.SetInteger(Constances.AnimationNumParameter, Random.Range(0, 3));
        }
        else
        {
            animator.SetBool(Constances.HasToJumpParameter, false);
        }
    }

    public void PlayFallAnimation()
    {
        PlayAnimation(false);
    }
}
