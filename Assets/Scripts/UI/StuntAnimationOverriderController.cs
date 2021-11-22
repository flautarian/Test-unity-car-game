using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntAnimationOverriderController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private AnimatorOverrideController animatorOverrider;

    private Animator anim;
    
    private void Start() {
        anim = GetComponent<Animator>();
        //animatorOverrider = new AnimatorOverrideController(animator.runtimeAnimatorController);
        anim.runtimeAnimatorController = animatorOverrider;
    }

    public void Set(AnimationClip animation){
        animatorOverrider["StuntDefaultAnimation"] = animation;
    }
}
