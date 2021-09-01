using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private bool isFrontWheel;

    private bool isLeftWheel;

    private ParticleSystem driftEffect;

    private float maxWheelTurn;

    [SerializeField]
    private PlayerController controller;

    private float verticalAxis, horizontalAxis, velocity;

    void Start()
    {
        driftEffect = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        if (isFrontWheel)
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, (controller.HorizontalAxis * maxWheelTurn) - (isLeftWheel? 180 : 0), transform.localRotation.eulerAngles.z);
        }
        if (controller.VerticalAxis != 0)
        {
            transform.Rotate(controller.getSpeedInput(), 0, 0, Space.Self);
        }
        if (driftEffect != null) manageDriftEffect();
        
    }

    private void manageDriftEffect()
    {
        var em = driftEffect.emission;
        if (controller.turnZAxisEffect != 0 && controller.grounded) em.enabled = true;
        else if(controller.turnZAxisEffect == 0 || !controller.grounded) em.enabled = false;
    }

}
