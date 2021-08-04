using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftWheelController : MonoBehaviour
{
    private ParticleSystem baseDriftParticle;

    void Start()
    {
        baseDriftParticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Drift(float turnZAxis, bool grounded, Color color)
    {
        if (turnZAxis != 0 && grounded) baseDriftParticle.Play();
        else if(turnZAxis == 0 || !grounded) baseDriftParticle.Pause();

        //baseDriftParticle.main.over = color;
    }
}
