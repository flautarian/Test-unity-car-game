using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CarController : MonoBehaviour {

	public float idealRPM = 500f;
	public float maxRPM = 1000f;

	public Transform centerOfGravity;

	public WheelCollider wheelFR;
	public WheelCollider wheelFL;
	public WheelCollider wheelRR;
	public WheelCollider wheelRL;

	public float VerticalAxis =0f, HorizontalAxis=0f;

	public float turnRadius = 6f;
	public float torque = 25f;
	public float brakeTorque = 100f;

	public float scaledTorque = 0f;

	public float AntiRoll = 20000.0f;

	private Rigidbody rBody;

	public enum DriveMode { Front, Rear, All };
	public DriveMode driveMode = DriveMode.Rear;

	void Start() {
		GetComponent<Rigidbody>().centerOfMass = centerOfGravity.localPosition;
		rBody = GetComponent<Rigidbody>();
	}

	public float Speed() {
		return wheelRR.radius * Mathf.PI * wheelRR.rpm * 60f / 1000f;
	}

	public float Rpm() {
		return wheelRL.rpm;
	}

	void FixedUpdate () {

		//Debug.Log ("Speed: " + Speed() + "km/h    RPM: " + wheelRL.rpm);
		HorizontalAxis = CaptureDirectionalKeys(HorizontalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT));
        VerticalAxis = CaptureDirectionalKeys(VerticalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_ACCELERATE), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_DOWN));
		scaledTorque = VerticalAxis * torque;

		if(wheelRL.rpm < idealRPM)
			scaledTorque = Mathf.Lerp(scaledTorque/10f, scaledTorque, wheelRL.rpm / idealRPM );
		else 
			scaledTorque = Mathf.Lerp(scaledTorque, 0,  (wheelRL.rpm-idealRPM) / (maxRPM-idealRPM) );
		
		DoRollBar(wheelFR, wheelFL);
		DoRollBar(wheelRR, wheelRL);

		wheelFR.steerAngle = HorizontalAxis * turnRadius;
		wheelFL.steerAngle = HorizontalAxis * turnRadius;

		wheelFR.motorTorque = driveMode==DriveMode.Rear  ? 0 : scaledTorque;
		wheelFL.motorTorque = driveMode==DriveMode.Rear  ? 0 : scaledTorque;
		wheelRR.motorTorque = driveMode==DriveMode.Front ? 0 : scaledTorque;
		wheelRL.motorTorque = driveMode==DriveMode.Front ? 0 : scaledTorque;

		wheelFR.brakeTorque = (VerticalAxis == 0f ? brakeTorque : 0f);
		wheelFL.brakeTorque = (VerticalAxis == 0f ? brakeTorque : 0f);
		wheelRR.brakeTorque = (VerticalAxis == 0f ? brakeTorque : 0f);
		wheelRL.brakeTorque = (VerticalAxis == 0f ? brakeTorque : 0f);
	}
	private float CaptureDirectionalKeys(float StartingPoint, KeyCode positive, KeyCode negative){
        if(!Input.GetKey(positive) && !Input.GetKey(negative)){
            if(Math.Abs(StartingPoint) < 0.05) StartingPoint = 0;
            else StartingPoint = Mathf.Lerp(0f, StartingPoint, 25 * Time.deltaTime);
        }
        else{
            StartingPoint += Input.GetKey(positive) ? (StartingPoint < 0f ? 0.1f : 0.05f) : 0.0f;
            StartingPoint += Input.GetKey(negative) ? (StartingPoint > 0f ? -0.1f : -0.05f) : 0.0f;
        }
        return Mathf.Clamp(StartingPoint, -1f, 1f);
    }

	void DoRollBar(WheelCollider WheelL, WheelCollider WheelR) {
		WheelHit hit;
		float travelL = 1.0f;
		float travelR = 1.0f;
		
		bool groundedL = WheelL.GetGroundHit(out hit);
		if (groundedL)
			travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;
		
		bool groundedR = WheelR.GetGroundHit(out hit);
		if (groundedR)
			travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;
		
		float antiRollForce = (travelL - travelR) * AntiRoll;
		
		if (groundedL)
			rBody.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
			                             WheelL.transform.position); 
		if (groundedR)
			rBody.AddForceAtPosition(WheelR.transform.up * antiRollForce,
			                             WheelR.transform.position); 
	}

}
