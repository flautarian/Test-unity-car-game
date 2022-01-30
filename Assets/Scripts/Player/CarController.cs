using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CarController : MonoBehaviour {


	public Transform centerOfGravity;

	private StuntsController stuntsController;

	public WheelCollider wheelFR;
	public WheelCollider wheelFL;
	public WheelCollider wheelRR;
	public WheelCollider wheelRL;
	public float idealRPM = 500f;
	public float maxRPM = 1000f;

	public float VerticalAxis =0f, HorizontalAxis=0f;

	public float turnRadius = 6f;
	public float torque = 25f;
	public float brakeTorque = 100f;
	public float actualBrake = 100f;
	public bool canMove = false;
	public bool turned = false;
	public bool grounded = false, groundedR = false, groundedL = false;
	public float scaledTorque = 0f;
	private float actualScaledTorque =0f;
	public float AntiRoll = 20000.0f;
	[SerializeField]
	internal Rigidbody rBody;

	public enum DriveMode { Front, Rear, All };
	public DriveMode driveMode = DriveMode.Rear;

	void Start() {
		stuntsController = GetComponent<StuntsController>();
		rBody.centerOfMass = centerOfGravity.localPosition;
	}

	public float Speed() {
		return wheelRR.radius * Mathf.PI * wheelRR.rpm * 60f / 1000f;
	}

	public float Rpm() {
		return wheelRL.rpm;
	}

	private void Update() {
		HorizontalAxis = CaptureDirectionalKeys(HorizontalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT));
        VerticalAxis = CaptureDirectionalKeys(VerticalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_ACCELERATE), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_DOWN));
	}

	void FixedUpdate () {
		Debug.Log ("Speed: " + Speed() + "km/h    RPM: " + wheelRL.rpm);
		scaledTorque = VerticalAxis * torque;

		if(scaledTorque >= 0){
			if(wheelRL.rpm < idealRPM)
				scaledTorque = Mathf.Lerp(scaledTorque/10f, scaledTorque, wheelRL.rpm / idealRPM );
			else 
				scaledTorque = Mathf.Lerp(scaledTorque, 0,  (wheelRL.rpm-idealRPM) / (maxRPM-idealRPM) );
		}
		
		DoRollBar(wheelFR, wheelFL);
		DoRollBar(wheelRR, wheelRL);

		actualScaledTorque = canMove ? scaledTorque : 0f;

		wheelFR.motorTorque = driveMode==DriveMode.Rear  ? 0 : actualScaledTorque;
		wheelFL.motorTorque = driveMode==DriveMode.Rear  ? 0 : actualScaledTorque;
		wheelRR.motorTorque = driveMode==DriveMode.Front ? 0 : actualScaledTorque;
		wheelRL.motorTorque = driveMode==DriveMode.Front ? 0 : actualScaledTorque;

		if(!stuntsController.stuntsModeEnabled){
			wheelFR.steerAngle = HorizontalAxis * turnRadius;
			wheelFL.steerAngle = HorizontalAxis * turnRadius;

			actualBrake = ((VerticalAxis < 0f && Rpm() > 0f) || (VerticalAxis > 0f && Rpm() < -20f) || !canMove ? brakeTorque : 0f);
			
			wheelFR.brakeTorque = actualBrake;
			wheelFL.brakeTorque = actualBrake;
			wheelRR.brakeTorque = actualBrake;
			wheelRL.brakeTorque = actualBrake;
		}

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
		
		groundedL = WheelL.GetGroundHit(out hit);
		if (groundedL)
			travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;
		
		groundedR = WheelR.GetGroundHit(out hit);
		if (groundedR)
			travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;
		
		float antiRollForce = (travelL - travelR) * AntiRoll;
		if(canMove){
			if (groundedL)
				rBody.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
											WheelL.transform.position); 
			if (groundedR)
				rBody.AddForceAtPosition(WheelR.transform.up * antiRollForce,
											WheelR.transform.position); 
		}
		
		//rBody.isKinematic = !canMove;

		grounded = groundedL || groundedR;
	}

	
    internal float GetVerticalAxis(){
        return VerticalAxis;
    }

    internal float GetHorizontalAxis(){
        return HorizontalAxis;
    }

    internal void impulseUpCar(float amount){
        rBody.AddForce(Vector3.up * amount, ForceMode.Impulse);
    }
    internal void impulseRightCar(float amount){
        rBody.AddForce(Vector3.right * amount, ForceMode.Impulse);
    }

    internal void impulseForwardCar(float amount){
        rBody.AddForce(Vector3.forward * amount, ForceMode.Impulse);
    }

    internal IEnumerator initializeNitro()
    {
        maxRPM += 200;
        yield return new WaitForSeconds(3f);
        maxRPM -= 200;
		stuntsController.UpdatePlayerAnimationBool(Constants.ANIMATION_NAME_NITRO_BOOL, false);
    }

}
