using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntAnimationOverriderController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private AnimatorOverrideController animatorOverrider;

    private Animator animator;
    
    private void Start() {
        animator = GetComponent<Animator>();
        //animatorOverrider = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrider;
    }

    public void Set(AnimationClip animation){
        animatorOverrider["StuntDefaultAnimation"] = animation;
    }
}
