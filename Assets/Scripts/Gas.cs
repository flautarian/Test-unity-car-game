using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : InteractableObject
{

    public Vector3 animPosition;
    public float value;
    private GasIndicator gasIndicator;

    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Timer") != null) 
           gasIndicator= GameObject.FindGameObjectWithTag("Timer").GetComponent<GasIndicator>();
    }

    void LateUpdate()
    {
        if (!animator) return;
        transform.localPosition += animPosition;
    }
    public override void Execute()
    {
        if(gasIndicator != null) 
            gasIndicator.AddSeconds(value);
    }
}
