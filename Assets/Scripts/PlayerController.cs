using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public StreetType streetType = StreetType.asphalt;
    public AudioClip turnUpCar;
    public AudioClip crashCar;

    public Vector3 com;
    public bool grounded = false;
    public LayerMask whatIsGround;
    public float groundRayLength;

    public void SphereEnterCollides(Collision collision)
    {
        if (System.Object.Equals(collision.gameObject.tag, "PlayerInteractable"))
        {
            collision.gameObject.GetComponent<Coin>().takeCoin();
        }
        else if (System.Object.Equals(collision.gameObject.layer, 8))// Ground
        {
            if (System.Object.Equals(collision.gameObject.tag, "Cesped"))
                streetType = StreetType.grass;
            else
                streetType = StreetType.asphalt;
        }
    }

    public Transform groundRayPoint;
    public float forwardAccel, reverseAccel, maxSpeed, turnStrength, gravityForce, dragGroundValue, maxWheelTurn;
    public Transform frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;
    public Rigidbody playerRigidbody;
    private float speedInput;
    private float VerticalAxis, HorizontalAxis;

    void Start()
    {
        playerRigidbody.transform.parent = null;
        Physics.IgnoreLayerCollision(0, 9);
    }

    // Update is called once per frame
    void Update()
    {
        VerticalAxis = Input.GetAxis("Vertical");
        if (VerticalAxis > 0) speedInput = VerticalAxis * forwardAccel * 1000f;
        else if (VerticalAxis < 0) speedInput = VerticalAxis * reverseAccel * 1000f;

        HorizontalAxis = Input.GetAxis("Horizontal");
        // position set
        transform.position = playerRigidbody.transform.position;
        // rotation set
        if (grounded && VerticalAxis > 0) transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3.up * HorizontalAxis * turnStrength * Time.deltaTime * VerticalAxis));

        // top wheels rotation set
        frontLeftWheel.localRotation = Quaternion.Euler(frontLeftWheel.localRotation.eulerAngles.x, (HorizontalAxis * maxWheelTurn) - 180, frontLeftWheel.localRotation.eulerAngles.z);
        frontRightWheel.localRotation = Quaternion.Euler(frontRightWheel.localRotation.eulerAngles.x, HorizontalAxis * maxWheelTurn, frontRightWheel.localRotation.eulerAngles.z);
        /*if (VerticalAxis != 0)
        {
            frontLeftWheel.Rotate(speedInput, 0, 0, Space.Self);
            frontRightWheel.Rotate(speedInput, 0, 0, Space.Self);
            rearLeftWheel.Rotate(speedInput, 0, 0, Space.Self);
            rearRightWheel.Rotate(speedInput, 0, 0, Space.Self);
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(com, new Vector3(0.25f, 0.25f, 0.25f));
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        grounded = false;
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        if (grounded)
        {
            playerRigidbody.drag = dragGroundValue;
            if (Math.Abs(speedInput) > 0) playerRigidbody.AddForce(transform.forward * speedInput);
        }
        else
        {
            playerRigidbody.drag = 0.1f;
            playerRigidbody.AddForce(Vector3.up * -gravityForce * 100f);
        }
    }

    public void EndGame()
    {

    }

    public void StartGame()
    {

    }
}
