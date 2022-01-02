using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExitIfNoInfiniteRunner : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(GlobalVariables.Instance.gameMode != GameMode.INFINITERUNNER)
            Destroy(animator.gameObject, stateInfo.length);
    }

}
