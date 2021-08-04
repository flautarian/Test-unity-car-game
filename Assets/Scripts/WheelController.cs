using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField]
    private bool isFrontWheel;

    [SerializeField]
    private bool isLeftWheel;

    private DriftWheelController driftWheel;

    [SerializeField]
    private float maxWheelTurn;

    [SerializeField]
    private PlayerController controller;

    private float verticalAxis, horizontalAxis, velocity;

    void Start()
    {
        driftWheel = GetComponentInChildren<DriftWheelController>(true);
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
        if (driftWheel != null) manageDriftEffect();
        
    }

    private void manageDriftEffect()
    {
        driftWheel.Drift(controller.turnZAxisEffect, controller.grounded, controller.getTouchingColor());
    }

}
