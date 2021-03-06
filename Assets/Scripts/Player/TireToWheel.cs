using UnityEngine;
using System.Collections;

public class TireToWheel : MonoBehaviour {

	public WheelCollider wheelCollider;

	void Start() {
		//wheelCollider.GetComponent<ParticleSystem>().emissionRate = 0;
	}

	void FixedUpdate () {
	//	transform.position = wheelCollider.su
		UpdateWheelHeight(this.transform, wheelCollider);
	}


	void UpdateWheelHeight(Transform wheelTransform, WheelCollider collider) {
		
		Vector3 localPosition = wheelTransform.localPosition;
		
		WheelHit hit = new WheelHit();
		
		// see if we have contact with ground
		
		if (collider.GetGroundHit(out hit)) {

			float hitY = collider.transform.InverseTransformPoint(hit.point).y;

			localPosition.y = hitY + collider.radius;
		} else {
			
			// no contact with ground, just extend wheel position with suspension distance

			localPosition = Vector3.Lerp (localPosition, -Vector3.up * collider.suspensionDistance, .05f);
			//wheelCollider.GetComponent<ParticleSystem>().enableEmission = false;

		}
		
		// actually update the position
		
		wheelTransform.localPosition = localPosition;

		wheelTransform.localRotation = Quaternion.Euler(0, collider.steerAngle, 0);
		wheelTransform.Rotate(collider.rpm, 0, 0);
		
	}


}
