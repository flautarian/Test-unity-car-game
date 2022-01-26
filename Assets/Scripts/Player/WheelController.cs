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
            /*if(wheelCollider != null){
                //transform.localPosition = Vector3.Lerp (transform.localPosition, wheelCollider.transform.InverseTransformPoint(hit.point).y + wheelCollider.radius, .05f);
                transform.localRotation = Quaternion.Euler(0, wheelCollider.steerAngle, 0);
            }*/

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
