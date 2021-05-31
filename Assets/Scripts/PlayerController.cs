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

    public ParticleSystem hitParticle;

    public bool grounded = false, canMove = true;

    public float groundRayLength;
    public float turnZAxisEffect = 0;
    public float forwardAccel, reverseAccel, maxSpeed, turnStrength, gravityForce, dragGroundValue, maxWheelTurn;

    public int coins;

    public LayerMask whatIsGround;

    public Transform groundRayPoint;

    internal void AddCoins(int number)
    {
        coins += number;
    }

    public Transform frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;

    public Rigidbody playerRigidbody;

    public List<GameObject> destructableParts = new List<GameObject>();
    
    private float speedInput;
    public float VerticalAxis, HorizontalAxis;

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
        
        HorizontalAxis = Input.GetAxis("Horizontal");
        // position set
        transform.position = playerRigidbody.transform.position;
        if (canMove)
        {
            // rotation set
            turnZAxisEffect = HorizontalAxis * (grounded ? 5 : 1);
            turnZAxisEffect = Mathf.Clamp(turnZAxisEffect, -5f, 5f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, HorizontalAxis * turnStrength * Time.deltaTime * VerticalAxis, turnZAxisEffect));
        }

        // top wheels rotation set
        frontLeftWheel.localRotation = Quaternion.Euler(frontLeftWheel.localRotation.eulerAngles.x, (HorizontalAxis * maxWheelTurn) - 180, frontLeftWheel.localRotation.eulerAngles.z);
        frontRightWheel.localRotation = Quaternion.Euler(frontRightWheel.localRotation.eulerAngles.x, HorizontalAxis * maxWheelTurn, frontRightWheel.localRotation.eulerAngles.z);
        if (VerticalAxis != 0)
        {
            frontLeftWheel.Rotate(speedInput, 0, 0, Space.Self);
            frontRightWheel.Rotate(speedInput, 0, 0, Space.Self);
            rearLeftWheel.Rotate(speedInput, 0, 0, Space.Self);
            rearRightWheel.Rotate(speedInput, 0, 0, Space.Self);
        }
    }

    private void FixedUpdate()
    {
            // Raycast
            RaycastHit hit;
            grounded = false;
            if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
            {
                grounded = true;
                transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            }

            if (grounded)
            {
            if (canMove)
                {
                    playerRigidbody.drag = dragGroundValue;
                    if (Math.Abs(speedInput) > 0 && VerticalAxis != 0) playerRigidbody.AddForce(transform.forward * speedInput);
                }
            }
            else
            {
                playerRigidbody.drag = 0.1f;
                playerRigidbody.AddForce(Vector3.up * -gravityForce * 100f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 1.0f * Time.deltaTime);
            }
    }
    public void SphereEnterCollides(Collision collision)
    {
        if (System.Object.Equals(collision.gameObject.tag, "PlayerInteractable"))
        {
            collision.gameObject.GetComponent<InteractableObject>().TakeObject(this);
        }
        else if (System.Object.Equals(collision.gameObject.layer, 8))// Ground
        {
            if (System.Object.Equals(collision.gameObject.tag, "Cesped"))
                streetType = StreetType.grass;
            else
                streetType = StreetType.asphalt;
        }
    }

    internal void communicatePlayerBaseCollition(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Obstaculo"))
        {
            GameObject partToDestroy = findPartNotDestroyed();
            if (partToDestroy != null) ComunicateCollisionPart(partToDestroy);
            else
            {
                //GAME OVER BY COLLITION
            }
        }
    }

    private GameObject findPartNotDestroyed()
    {
        foreach(GameObject part in destructableParts)
        {
            if (!part.GetComponent<PlayerDestructablePart>().destroyed) return part;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawCube(com, new Vector3(0.25f, 0.25f, 0.25f));
    }
    internal void ComunicateCollisionPart(GameObject partDestroyed)
    {
        if (!GetComponent<Animator>().GetBool("hit"))
        {
            hitParticle.transform.position = partDestroyed.transform.position;
            hitParticle.Play();
            GetComponent<Animator>().SetBool("hit", true);
            GameObject falseDestroyPart = Instantiate(partDestroyed);
            falseDestroyPart.transform.parent = null;
            falseDestroyPart.GetComponent<PlayerDestructablePart>().ejectPart(partDestroyed);
            partDestroyed.GetComponent<PlayerDestructablePart>().Inhabilite();
        }
    }

    internal void RecoverParts()
    {
        foreach (GameObject dp in destructableParts) {
            if(dp.GetComponent<PlayerDestructablePart>().destroyed) 
                dp.GetComponent<PlayerDestructablePart>().Recover();
        }
    }
    public void EndGame()
    {
        canMove = false;
    }

    public void StartGame()
    {
        canMove = true;
    }
}
