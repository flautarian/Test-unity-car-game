using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Assets.Scripts;

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

	public StreetType currentStreetType = StreetType.asphalt;

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
		HorizontalAxis = CaptureDirectionalKeys(HorizontalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT), 0.1f, false);
        VerticalAxis = CaptureDirectionalKeys(VerticalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_ACCELERATE), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_DOWN), 0.1f, true);
		
	}

	void FixedUpdate () {
		// Debug.Log ("Speed: " + Speed() + "km/h    RPM: " + wheelRL.rpm);
		// wheel running
		scaledTorque = VerticalAxis * (torque + stuntsController.GetComboStunt());

		if(scaledTorque >= 0){
			if(wheelRL.rpm < idealRPM)
				scaledTorque = Mathf.Lerp(scaledTorque/10f, scaledTorque, wheelRL.rpm / idealRPM );
			else 
				scaledTorque = Mathf.Lerp(scaledTorque, 0,  (wheelRL.rpm-idealRPM) / (maxRPM-idealRPM) );
		}
		
		DoRollBar(wheelFR, wheelFL, true);
		DoRollBar(wheelRR, wheelRL, false);

		actualScaledTorque = canMove ? scaledTorque : 0f;

		wheelFR.motorTorque = (driveMode== DriveMode.Rear  ? 0 : actualScaledTorque);
		wheelFL.motorTorque = (driveMode== DriveMode.Rear  ? 0 : actualScaledTorque);
		wheelRR.motorTorque = (driveMode== DriveMode.Front ? 0 : actualScaledTorque);
		wheelRL.motorTorque = (driveMode== DriveMode.Front ? 0 : actualScaledTorque);

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
	
	private float CaptureDirectionalKeys(float StartingPoint, KeyCode positive, KeyCode negative, float axisSensibility, bool isforwardAccel){
		if(isforwardAccel){	
			if(Input.GetKey(positive) && Input.GetKey(negative) && rBody.velocity.magnitude < 5){
				rBody.velocity = Vector3.zero;
				stuntsController.UpdatePlayerAnimationBool(Constants.ANIMATION_TRIGGER_PLAYER_PREPARE_ACCELERATION, true);
				return 0;
			}
			else stuntsController.UpdatePlayerAnimationBool(Constants.ANIMATION_TRIGGER_PLAYER_PREPARE_ACCELERATION, false);
		}
        if(!Input.GetKey(positive) && !Input.GetKey(negative)){
            if(Math.Abs(StartingPoint) < 0.05) StartingPoint = 0;
            else StartingPoint = Mathf.Lerp(0f, StartingPoint, 25 * Time.deltaTime);
        }
        else{
            StartingPoint += Input.GetKey(positive) ? (StartingPoint < 0f ? axisSensibility : axisSensibility) : 0.0f;
            StartingPoint += Input.GetKey(negative) ? (StartingPoint > 0f ? -axisSensibility : -axisSensibility) : 0.0f;
        }
        return Mathf.Clamp(StartingPoint, -1f, 1f);
    }

	void DoRollBar(WheelCollider WheelL, WheelCollider WheelR, bool calculateLanding) {
		WheelHit hit;
		float travelL = 1.0f;
		float travelR = 1.0f;
		
		groundedL = WheelL.GetGroundHit(out hit);
		
		if(hit.collider != null)
			Enum.TryParse(hit.collider.gameObject.tag, out currentStreetType);

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
		if((groundedL || groundedR) && !grounded && calculateLanding)
			GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
		grounded = groundedL || groundedR;
	}

	
    internal float GetVerticalAxis(){
        return VerticalAxis;
    }

    internal float GetHorizontalAxis(){
        return HorizontalAxis;
    }

    internal void impulseUpCar(float amount){
        rBody.AddRelativeForce(Vector3.up * amount, ForceMode.Impulse);
    }

	internal void explodeCar(){
		GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_EXPLOSION, transform.position);
	}

    internal void impulseRightCar(float amount){
        rBody.AddRelativeForce(Vector3.right * amount, ForceMode.Impulse);
    }

    internal void impulseForwardCar(float amount){
		GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
        rBody.AddRelativeForce(Vector3.forward * amount, ForceMode.Acceleration);
		stuntsController.UpdatePlayerAnimationInt(Constants.ANIMATION_TRIGGER_PLAYER_ACCELERATION_INT,0);
    }

	internal void TriggerHatTrick(){
        Player player = GetComponentInParent(typeof(Player)) as Player;
	}

    internal void addClockAccel(int amount){
		int res = stuntsController.GetPlayerAnimationInt(Constants.ANIMATION_TRIGGER_PLAYER_ACCELERATION_INT);
        stuntsController.UpdatePlayerAnimationInt(Constants.ANIMATION_TRIGGER_PLAYER_ACCELERATION_INT, res + amount >= 100 ? 100 :  res + amount);
		GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
    }

    internal void TogglePlayerObstaclesClip(int value){
        Physics.IgnoreLayerCollision(0, 9, value == 1);
        Physics.IgnoreLayerCollision(9, 11, value == 1);
    }


    internal IEnumerator initializeNitro()
    {
        maxRPM += 200;
        yield return new WaitForSeconds(3f);
        maxRPM -= 200;
		stuntsController.UpdatePlayerAnimationBool(Constants.ANIMATION_NAME_NITRO_BOOL, false);
    }

}
