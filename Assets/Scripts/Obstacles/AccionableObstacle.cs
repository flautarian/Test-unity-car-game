using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AccionableObstacle : MonoBehaviour
{
    internal Animator actionableAnimator;

    private void Start()
    {
        actionableAnimator = GetComponent<Animator>();
    }
    public abstract void executeAction();
    public abstract void executeStopAction();
}
