using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntIconController : MonoBehaviour
{
    internal Animator animator;
    internal int keyCode = -1;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update() {
        var state = GlobalVariables.Instance.castingStunt;
        if(state == StuntState.STUNTWRONG || state == StuntState.OFF)
            animator.SetBool(Constants.ANIMATION_NAME_STUNT_WRONG_ICON_BOOL, true);
        else if(state == StuntState.STUNTCOMPLETED)
            animator.SetBool(Constants.ANIMATION_NAME_STUNT_STUNT_COMPLETED_ICON_BOOL, true);
    }
}
