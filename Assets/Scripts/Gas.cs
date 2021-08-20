﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : InteractableObject
{

    public Vector3 animPosition;
    public float value;

    void LateUpdate()
    {
        if (!GetComponent<Animator>().GetBool("hasBeenTaken")) return;
        transform.localPosition += animPosition;
    }
    public override void Execute(PlayerController controller)
    {
        if(GameObject.FindGameObjectWithTag("Timer") != null) 
            GameObject.FindGameObjectWithTag("Timer").GetComponent<GasIndicator>().AddSeconds(value);
    }
}
