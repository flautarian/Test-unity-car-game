using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip turnUpCar;
    public AudioClip crashCar;
    public float life;
    public float maxVelocity;
    public float statusCarVelocity;
    public float acceleration;
    public float velocity;
    public float round;

    public Vector3 com;
    public bool grounded = false;
    public LayerMask whatIsGround;
    public float groundRayLength;
    public Transform groundRayPoint;
    public float forwardAccel, reverseAccel, maxSpeed, turnStrength, gravityForce, dragGroundValue, maxWheelTurn;
    public Transform frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;
    public Rigidbody playerRigidbody;
    private float speedInput, turnInput;
    private float VerticalAxis, HorizontalAxis;


    private float minX = -8f;
    private float maxX = 8f;

    private Vector2 target;
    private bool endGameFlag = false;
    private bool startGameFlag = false;

    GameObject motor;

    void Start()
    {
        motor = GameObject.FindGameObjectWithTag("motorCarreteras");
        playerRigidbody.transform.parent = null;
        Physics.IgnoreLayerCollision(0, 9);
    }

    

    // Update is called once per frame
    void Update()
    {
        //controlVelocity();
        //controlPlayer();
        //moveWheels(velocity);
        VerticalAxis = Input.GetAxis("Vertical");
        if (VerticalAxis > 0) speedInput = VerticalAxis * forwardAccel * 1000f;
        else if(VerticalAxis < 0) speedInput = VerticalAxis * reverseAccel * 1000f;

        HorizontalAxis = Input.GetAxis("Horizontal");
        // position set
        transform.position = playerRigidbody.transform.position;
        // rotation set
        if (grounded && VerticalAxis > 0)transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3.up * HorizontalAxis * turnStrength * Time.deltaTime * VerticalAxis));

        // top wheels rotation set
        frontLeftWheel.localRotation = Quaternion.Euler(frontLeftWheel.localRotation.eulerAngles.x, (HorizontalAxis * maxWheelTurn) - 180, frontLeftWheel.localRotation.eulerAngles.z);
        frontRightWheel.localRotation = Quaternion.Euler(frontRightWheel.localRotation.eulerAngles.x, HorizontalAxis * maxWheelTurn, frontRightWheel.localRotation.eulerAngles.z);
        if(VerticalAxis != 0)
        {
            frontLeftWheel.Rotate(speedInput, 0, 0, Space.Self);
            frontRightWheel.Rotate(speedInput, 0, 0, Space.Self);
            rearLeftWheel.Rotate(speedInput, 0, 0, Space.Self);
            rearRightWheel.Rotate(speedInput, 0, 0, Space.Self);
        }
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

    internal void executePowerUp(string powerUpButtonName)
    {
        switch (powerUpButtonName)
        {
            case "ThunderButtton":
                break;
            case "StarButtton":
                break;
            case "BulletButtton":
                break;
            default:
                break;
        }
    }


    internal void ContactedWithGrass()
    {
        this.velocity = this.statusCarVelocity / 2;
    }



    private void controlVelocity()
    {
        if (!endGameFlag){
            if (statusCarVelocity > velocity) velocity += acceleration;
        }
        else reduceVelocityToZero();
    }

    public void endGame()
    {
        this.endGameFlag = true;
        motor.GetComponent<MotorCarreteras>().endGame();
    }

    public void startGame()
    {
        this.startGameFlag = true;
    }

    private void controlPlayer()
    {
        // ACCELERATION
        if (Input.GetKey("up"))
        {
            if (!endGameFlag)
            {
                if (statusCarVelocity > velocity) velocity += acceleration;
            }
        }
        else reduceVelocityToZero();


        // COMMAND CAR
        if (IsGrounded())
        {
            if (Input.GetKey("left") && IsGrounded())
            {
                GetComponent<Transform>().Rotate(Vector3.down * round * Time.deltaTime);
            }
            else if (Input.GetKey("right") && IsGrounded())
            {
                GetComponent<Transform>().Rotate(Vector3.up * round * Time.deltaTime);
            }
            else GetComponent<Transform>().rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 3.0f * Time.deltaTime);
            if(Math.Abs(GetComponent<Transform>().rotation.z) > 10.0f) GetComponent<Transform>().rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f), round * Time.deltaTime);
        }
        else GetComponent<Transform>().rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f), 8.5f * Time.deltaTime);


        target = Input.GetAxis("Horizontal") * Vector3.forward * velocity * Time.deltaTime;

        //acceleration
        if (startGameFlag)
        {
            if (transform.position.x + target.x <= minX && transform.position.x + target.x < maxX)
            {
                Vector3 eulerRotation = transform.rotation.eulerAngles;
                GetComponent<Transform>().rotation = Quaternion.Euler(-eulerRotation.x, eulerRotation.y, eulerRotation.z);
            }
            GetComponent<Transform>().Translate(Vector3.forward * velocity * Time.deltaTime);
            
        }      
    }

    private void reduceVelocityToZero()
    {
        if (velocity > 1) velocity -= Time.deltaTime * velocity;
        else velocity = 0;
    }

    private void enablePowerUpButton(string v)
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Buttons");
        foreach(GameObject button in buttons)
        {
            if (v.Equals(button.name)){
                button.GetComponent<PowerUpButton>().enable();
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (System.Object.Equals(collision.gameObject.tag, "PlayerInteractable"))
        {
            collision.gameObject.GetComponent<Coin>().takeCoin();
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(groundRayPoint.position, -transform.up, groundRayLength, whatIsGround);
    }

private void OnTriggerEnter(Collider collision)
    {
        if (System.Object.Equals(collision.gameObject.tag, "PlayerInteractable"))
        {
            if (collision.gameObject.GetComponent<Coin>() != null) collision.gameObject.GetComponent<Coin>().takeCoin();
            else life -= 1;
        }
    }
}
