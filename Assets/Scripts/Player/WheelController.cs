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
    private Player player;

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
        var objPlayer = GameObject.FindGameObjectWithTag(Constants.GO_TAG_PLAYER);
        if (objPlayer != null && objPlayer.TryGetComponent(out Player p))
            player = p;
        if(driftEffect != null){
            driftPSEmissionVar = driftEffect.emission;
            driftEffect.Stop();
        }
        tParent = transform.parent;
    }

    void FixedUpdate()
    {
        if(grounded && driftEffect != null)
            manageDriftEffect();
        if(player.actualWheel != null && wheelIndex != player.actualWheel.keyCode)
            UpdateWheel(player.actualWheel);
        UpdateWheelHeight(this.transform, wheelCollider);
    }

    private void manageDriftEffect()
    {
        driftPSEmissionVar.enabled = (player.carController.GetHorizontalAxis() != 0 || (player.carController.GetVerticalAxis() > 0 && player.carController.GetVerticalAxis() < 1));
        if(driftPSEmissionVar.enabled && !driftEffect.isPlaying)driftEffect.Play();
    }

    private void UpdateWheel(ShopWheel wheel){
        meshFilter.sharedMesh = wheel.CWheel;
        transform.localScale = new Vector3(wheel.wheelSize, wheel.wheelSize, wheel.wheelSize);
        wheelCollider.radius = wheel.wheelSize / 2;
        wheelIndex = wheel.keyCode;
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
