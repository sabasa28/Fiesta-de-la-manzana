using System.Collections;
using UnityEngine;

public class MascotAnimationController : MonoBehaviour
{
    Animator animator;

    public enum MascotAnimations
    {
        Idle,
        IdleHand,
        Crying,
        ClosedEyesSad,
        ClosedEyesHappy,
        ClosedEyesHappyHand
    }

    public MascotAnimations initialAnimation; 
    void Start()
    {
        animator = GetComponent<Animator>();    
        PlayAnim(initialAnimation);
    }

    public void PlayAnim(MascotAnimations anim)
    {
        switch (anim)
        {
            case MascotAnimations.Idle:
                animator.Play("Idle");
                break;
            case MascotAnimations.IdleHand:
                animator.Play("IdleHand");
                break;
            case MascotAnimations.Crying:
                animator.Play("Crying");

                break;
            case MascotAnimations.ClosedEyesSad:
                animator.Play("ClosedEyesSad");
                break;
            case MascotAnimations.ClosedEyesHappy:
                animator.Play("ClosedEyesHappy");
                break;
            case MascotAnimations.ClosedEyesHappyHand:
                animator.Play("ClosedEyesHappyHand");
                break;
        }
    }
}
