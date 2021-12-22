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
    [SerializeField]
    private ParticleSystem driftEffect;

    private float maxWheelTurn;

    [SerializeField]
    private PlayerController controller;

    private float verticalAxis, horizontalAxis, velocity;

    private ParticleSystem.EmissionModule driftPSEmissionVar;

    [SerializeField]
    private Transform tParent;

    void Start()
    {
        driftEffect = GetComponent<ParticleSystem>();
        if(driftEffect != null){
            driftPSEmissionVar = driftEffect.emission;
            driftEffect.Stop();
        }
        tParent = transform.parent;
    }

    void Update()
    {
        if(controller != null){
            if (isFrontWheel)
            {
                tParent.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, (controller.HorizontalAxis * maxWheelTurn) - (isLeftWheel ? 180 : 0), transform.localRotation.eulerAngles.z);
            }
            if (controller.VerticalAxis != 0)
            {
                transform.Rotate(controller.getSpeedInput(), 0, 0, Space.Self);
            }
            if (driftEffect != null) manageDriftEffect();
        }
        else {
            Player player = GameObject.FindGameObjectWithTag(Constants.GO_TAG_PLAYER).GetComponent<Player>();
            if(player != null) controller = player.controller;
        }
    }

    private void manageDriftEffect()
    {
        driftPSEmissionVar.enabled = (controller.turnZAxisEffect != 0 || (controller.VerticalAxis > 0 && controller.VerticalAxis < 1)) && controller.grounded;
        if(driftPSEmissionVar.enabled && !driftEffect.isPlaying)driftEffect.Play();
    }

}
