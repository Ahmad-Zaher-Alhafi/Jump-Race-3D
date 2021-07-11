using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private AnimationClip winAnimationClip;
    [SerializeField] private AnimationClip loseAnimationClip;
    [SerializeField] private AnimationClip warmingupAnimationClip;
    [SerializeField] private AnimationClip[] jumpAnimationClips;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //public void SetAnimatorParameter(bool hasToJump)
    //{
    //    if (animator == null)
    //    {
    //        animator = GetComponent<Animator>();
    //    }

    //    if (hasToJump)
    //    {
    //        animator.SetBool(Constances.HasToJumpParameter, true);
    //        animator.SetInteger(Constances.AnimationNumParameter, Random.Range(0, 3));
    //    }
    //    else
    //    {
    //        animator.SetBool(Constances.HasToJumpParameter, false);
    //    }
    //}

    public void playAnimation(Constances.AnimationsTypes animationType)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        switch (animationType)
        {
            case Constances.AnimationsTypes.Win:
                animator.Play(winAnimationClip.name);
                break;
            case Constances.AnimationsTypes.Warmingup:
                animator.Play(warmingupAnimationClip.name);
                break;
            case Constances.AnimationsTypes.Lose:
                animator.Play(loseAnimationClip.name);
                break;
            case Constances.AnimationsTypes.Jump:
                animator.Play(jumpAnimationClips[Random.Range(0, 3)].name);
                break;
            default:
                break;
        }
    }

    //public void PlayFallAnimation()
    //{
    //    SetAnimatorParameter(false);
    //}
}