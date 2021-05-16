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
    public float statusCarVelocity;
    public float acceleration;
    public float velocity;
    public float steeringAngle;
    public float round;

    public float maxSteeringAngle = 30;
    public float motorForce;
    public float verticalInput, horizontalInput;

    public WheelCollider frontDriverWheel, frontPassengerWheel;
    public WheelCollider rearDriverWheel, rearPassengerWheel;

    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public Vector3 centerOfMass;


    private bool playerIsTouchingEarth = false;
    private Rigidbody playerRigidbody;
    private float minX = -8f;
    private float maxX = 8f;

    private Vector2 target;
    private bool endGameFlag = false;
    private bool startGameFlag = false;

    GameObject motor;

    void Start()
    {
        motor = GameObject.FindGameObjectWithTag("motorCarreteras");
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.centerOfMass = centerOfMass;
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

    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    private void Steer()
    {
        steeringAngle = maxSteeringAngle * horizontalInput;
        frontDriverWheel.steerAngle = steeringAngle;
        frontPassengerWheel.steerAngle = steeringAngle;
    }
    private void Accelerate()
    {
        frontDriverWheel.motorTorque = verticalInput * motorForce;
        frontPassengerWheel.motorTorque = verticalInput * motorForce;
        rearDriverWheel.motorTorque = verticalInput * motorForce;
        rearPassengerWheel.motorTorque = verticalInput * motorForce;
    }
    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverWheel, frontDriverT);
        UpdateWheelPose(frontPassengerWheel, frontPassengerT);
        UpdateWheelPose(rearDriverWheel, rearDriverT);
        UpdateWheelPose(rearPassengerWheel, rearPassengerT);

    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;
        _collider.GetWorldPose(out _pos, out _quat);
        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    internal void ContactedWithGrass()
    {
        this.velocity = this.statusCarVelocity / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*controlVelocity();
        controlPlayer();*/
        playerRigidbody.centerOfMass = centerOfMass;
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        //moveWheels(velocity);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * playerRigidbody.centerOfMass, 0.1f);
    }


    private void controlVelocity()
    {
        if (!endGameFlag){
            if (statusCarVelocity > velocity) velocity += acceleration;
        }
        else reduceVelocityToZero();
    }

    private void moveWheels(float velocity)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).Rotate(Vector3.right * Time.deltaTime * velocity * 50, Space.Self);
        }
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
            GetComponent<Rigidbody>().AddForce(transform.forward * velocity);
            //GetComponent<Transform>().Translate(Vector3.forward * velocity * Time.deltaTime);
            
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

    private bool IsGrounded(){
        return Physics.Raycast(transform.position, -Vector3.up, 0.25f);
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
