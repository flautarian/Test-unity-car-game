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

    private MeshFilter meshFilter;

    private int wheelIndex = 0;

    [SerializeField]
    private PlayerController controller;

    private float verticalAxis, horizontalAxis, velocity;

    private ParticleSystem.EmissionModule driftPSEmissionVar;

    [SerializeField]
    private Transform tParent;

    void Start()
    {
        driftEffect = GetComponent<ParticleSystem>();
        meshFilter = GetComponent<MeshFilter>();
        if(driftEffect != null){
            driftPSEmissionVar = driftEffect.emission;
            driftEffect.Stop();
        }
        tParent = transform.parent;
    }

    void Update()
    {
        if(controller != null){
            if(controller.wheel != null && controller.wheel.keyCode != wheelIndex){
                wheelIndex = controller.wheel.keyCode;
                meshFilter.sharedMesh = controller.wheel.CWheel;
                transform.localScale = Vector3.one * controller.wheel.wheelSize;
            }
            
            tParent.localRotation = Quaternion.Euler(tParent.localRotation.eulerAngles.x, (isFrontWheel ? controller.HorizontalAxis : 0f) * controller.maxWheelTurn, tParent.localRotation.eulerAngles.z);
            
            if (controller.VerticalAxis != 0)
            {
                transform.Rotate(controller.VerticalAxis * controller.forwardAccel , 0f, 0f, Space.Self);
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
