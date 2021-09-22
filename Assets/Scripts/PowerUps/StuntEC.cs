using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntEC : InteractableObject
{
    public Vector3 animPosition;
    public int value;

    void LateUpdate()
    {
        if (!animator.GetBool(Constants.ANIMATION_NAME_TAKEN_BOOL)) return;
        transform.localPosition += animPosition;
    }

    public override void Execute()
    {
        GlobalVariables.Instance.addStuntEC(value);
    }
}
