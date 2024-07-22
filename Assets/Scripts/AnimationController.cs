using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    string currentAnimation = null;
    Animator animator;
    float currentAnimationPriority = 0;
    string animationToPlayAfterReset = null;

    string animationToStartAtEndOfFrame = null;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        currentAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    private void OnEnable()
    {
        animator.Play(currentAnimation);
    }

    public string GetAnimationName()
    {
        return currentAnimation;
    }

    // Note: Animations with higher priority can never be changed to animations with lower priority unless StopAndResetPriority() is called
    // e.g. shooting while running has priority 1, so it will finish before the normal run animation starts again. At the end of the shooting
    // animation, StopAndResetPriority() is called
    public void SetAnimationState(string animation, float priority = 0)
    {
        // Usefull debugging printout:
        /*Debug.Log("animation: " + animation
            + "    priority: " + priority.ToString()
            + "    too low priority: " + (priority < currentAnimationPriority).ToString()
            + "    currently playing: " + (currentAnimation == animation).ToString());*/

        if (priority < currentAnimationPriority)
        {
            animationToPlayAfterReset = animation;
            return;
        }
        else
        {
            animationToPlayAfterReset = currentAnimation;
        }

        currentAnimationPriority = priority;
        if (currentAnimation == animation) return;

        currentAnimation = animation;
        animationToStartAtEndOfFrame = animation;
    }

    // This is needed because the behavior of animation player is bad: 
    // https://forum.unity.com/threads/bug-or-misunderstanding-animator-play-statename-called-multiple-times-in-one-frame-has-issue.816462/
    private void LateUpdate()
    {
        if (animationToStartAtEndOfFrame != null)
        {
            if (animationToStartAtEndOfFrame != "" && GetClip(animationToStartAtEndOfFrame) == null)
            {
                Debug.LogError(animationToStartAtEndOfFrame + " is not a valid animation for this controller.");
            }

            animator.Play(animationToStartAtEndOfFrame);
            animationToStartAtEndOfFrame = null;
        }
    }

    public void StopAndResetPriority()
    {
        currentAnimationPriority = 0;
        SetAnimationState(animationToPlayAfterReset);
    }

    public void StopAndMoveToState(string animation)
    {
        StopAndResetPriority();
        SetAnimationState(animation);
    }

    public AnimationClip GetClip(string animation)
    {
        foreach (AnimationClip c in animator.runtimeAnimatorController.animationClips)
        {
            if (c.name == animation)
            {
                return c;
            }
        }
        return null;
    }

    public void PlayAnimationOnce(string animation, float priority = 1)
    {
        StartCoroutine(PlayAnimationOnceCoroutine(animation, priority));
    }

    public IEnumerator PlayAnimationOnceCoroutine(string animation, float priority = 1)
    {
        SetAnimationState(animation, priority);
        AnimationClip clip = GetClip(animation);
        float animationLength = clip.length;
        yield return new WaitForSeconds(animationLength);
        StopAndResetPriority();
    }
}