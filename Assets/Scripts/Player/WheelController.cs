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
    [SerializeField]
    private WheelCollider wheelCollider;
    private bool grounded = false;

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

    void FixedUpdate()
    {
        if(controller == null){
            Player player = GameObject.FindGameObjectWithTag(Constants.GO_TAG_PLAYER).GetComponent<Player>();
            if(player != null) controller = player.controller;
        }
        if(grounded && controller != null && driftEffect != null)
            manageDriftEffect();
        UpdateWheelHeight(this.transform, wheelCollider);
    }

    private void manageDriftEffect()
    {
        driftPSEmissionVar.enabled = (controller.GetHorizontalAxis() != 0 || (controller.GetVerticalAxis() > 0 && controller.GetVerticalAxis() < 1));
        if(driftPSEmissionVar.enabled && !driftEffect.isPlaying)driftEffect.Play();
    }

    
	void UpdateWheelHeight(Transform wheelTransform, WheelCollider collider) {
		
		Vector3 localPosition = wheelTransform.localPosition;
		
		WheelHit hit = new WheelHit();
		
		// see if we have contact with ground
		
		if (collider.GetGroundHit(out hit)) {
			float hitY = collider.transform.InverseTransformPoint(hit.point).y;
			localPosition.y = hitY + collider.radius;
            grounded = true;
		} else {
			// no contact with ground, just extend wheel position with suspension distance
            grounded = false;
			localPosition = Vector3.Lerp (localPosition, -Vector3.up * collider.suspensionDistance, .05f);
		}
		
		// actually update the position
		
		wheelTransform.localPosition = localPosition;

		wheelTransform.localRotation = Quaternion.Euler(0, collider.steerAngle, 0);
		wheelTransform.Rotate(collider.rpm, 0, 0);
		
	}

}
