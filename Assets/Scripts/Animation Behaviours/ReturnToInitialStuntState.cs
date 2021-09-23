using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToInitialStuntState : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(Constants.ANIMATION_NAME_CAST_STUNT_INT, -1);    
    }
}
