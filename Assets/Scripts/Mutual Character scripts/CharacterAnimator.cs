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
}