using Assets.Scripts;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public StreetType streetType = StreetType.asphalt;

    public AudioClip turnUpCar;

    public AudioClip crashCar;

    public ParticleSystem hitParticle;

    public ParticleSystem smokeHitParticle;

    public ParticleSystem landingParticle;

    public GUIController guiPlayer;

    public bool grounded = false, canMove = false;

    public float groundRayLength;

    public float turnZAxisEffect = 0;

    public float forwardAccel, reverseAccel, maxSpeed, turnStrength, gravityForce, dragGroundValue, maxWheelTurn;

    public LayerMask whatIsGround;

    public Transform groundRayPoint;

    public Rigidbody playerSphereRigidBody;

    public BoxCollider playerBoxCollider;

    public List<GameObject> destructableParts = new List<GameObject>();

    private Color touchingColor;

    private float speedInput;

    public float VerticalAxis, HorizontalAxis;

    void Start()
    {
        playerSphereRigidBody.transform.parent = null;
        Physics.IgnoreLayerCollision(0, 9);
    }

    // Update is called once per frame
    void Update()
    {
        //Activem l'animacio de 'vehicle xocat' en cas de haver xocat amb el vehicle
        if (GetComponent<Animator>().GetBool("hit") && !playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = true;
        else if (!GetComponent<Animator>().GetBool("hit") && playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = false;

        //Captura de tecles
        VerticalAxis = Input.GetAxis("Vertical");
        if (VerticalAxis > 0) speedInput = VerticalAxis * forwardAccel * 1000f;
        HorizontalAxis = Input.GetAxis("Horizontal");
        
        //Refresc de posició
        transform.position = playerSphereRigidBody.transform.position;
        if (canMove)
        {
            turnZAxisEffect = HorizontalAxis * (grounded ? 5 : 1);
            turnZAxisEffect = Mathf.Clamp(turnZAxisEffect, -5f, 5f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, HorizontalAxis * turnStrength * Time.deltaTime * VerticalAxis, turnZAxisEffect));
        }

    }


    internal void turnLeft()
    {
        throw new NotImplementedException();
    }

    internal void turnRight()
    {
        throw new NotImplementedException();
    }

    private void FixedUpdate()
    {
        // Raycast
        RaycastHit hitRayCast;
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hitRayCast, groundRayLength, whatIsGround))
        {
            if (!grounded) landingParticle.gameObject.SetActive(true);
            grounded = true;
        }
        else grounded = false;
        transform.rotation = Quaternion.FromToRotation(transform.up, hitRayCast.normal) * transform.rotation;

        if (grounded && canMove)
        {
            playerSphereRigidBody.drag = dragGroundValue;
            if (Math.Abs(speedInput) > 0 && VerticalAxis != 0) playerSphereRigidBody.AddForce(transform.forward * speedInput);
        }
        else
        {
            playerSphereRigidBody.drag = 0.1f;
            playerSphereRigidBody.AddForce(Vector3.up * -gravityForce * 100f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 1.0f * Time.deltaTime);
        }
        if (!canMove) playerSphereRigidBody.drag = 3.5f;

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
            else if(System.Object.Equals(collision.gameObject.tag, "asphalt"))
                streetType = StreetType.asphalt;
            else if (System.Object.Equals(collision.gameObject.tag, "water"))
            {
                streetType = StreetType.water;
                destroyPlayer("Vehicle drowned");
            }
                
        }
    }
    private void destroyPlayer(string reason)
    {
        GetComponent<Animator>().SetBool("explode", true);
        GameObject gui = GameObject.FindGameObjectWithTag("GUI");
        if (gui != null) gui.GetComponent<GUIController>().startGameOver(reason);
    }

    internal void AddCoins(int number)
    {
        if (guiPlayer != null) guiPlayer.addCoins(number);
    }

    internal void communicatePlayerBaseCollition(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Obstaculo"))
        {
            GameObject partToDestroy = findPartNotDestroyed();
            if (partToDestroy != null) ComunicateCollisionPart(partToDestroy, collision.collider);
            else
            {
                destroyPlayer("Vehicle destroyed");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawCube(com, new Vector3(0.25f, 0.25f, 0.25f));
    }
    private GameObject findPartNotDestroyed()
    {
        //return destructableParts.Where(part => !part.GetComponent<PlayerDestructablePart>().destroyed).First();
        foreach(GameObject part in destructableParts)
        {
            if (!part.GetComponent<PlayerDestructablePart>().destroyed) return part;
        }
        return null;
    }
    internal void ComunicateCollisionPart(GameObject partDestroyed, Collider collision)
    {
        int indexPartToDestroy = destructableParts.IndexOf(partDestroyed);
        GlobalVariables.Instance.currentBrokenScreen = 0.08f * (indexPartToDestroy + 1);
        GlobalVariables.Instance.shakeParam += 3f;
        if (!GetComponent<Animator>().GetBool("hit"))
        {
            GetComponent<Animator>().SetBool("hit", true);
            hitParticle.transform.position = partDestroyed.transform.position;
            hitParticle.gameObject.SetActive(true);
            smokeHitParticle.transform.position = partDestroyed.transform.position;
            smokeHitParticle.gameObject.SetActive(true);
            if (guiPlayer != null)
            {
                GameObject partsGUI = guiPlayer.GetComponent<GUIController>().carPartsIndicator;
                if (partsGUI != null) partsGUI.GetComponent<CarPartsIndicator>().decrementPart();
            }
            GameObject falseDestroyPart = Instantiate(partDestroyed);
            falseDestroyPart.transform.parent = null;
            falseDestroyPart.GetComponent<PlayerDestructablePart>().ejectPart(partDestroyed);
            collision.gameObject.GetComponent<Obstacle>().Collide(partDestroyed.transform);
            partDestroyed.GetComponent<PlayerDestructablePart>().Inhabilite();
        }
    }

    internal void RecoverParts()
    {
        foreach (GameObject dp in destructableParts) {
            if(dp.GetComponent<PlayerDestructablePart>().destroyed) 
                dp.GetComponent<PlayerDestructablePart>().Recover();
        }
        if (guiPlayer != null)
        {
            GameObject partsGUI = guiPlayer.GetComponent<GUIController>().carPartsIndicator;
            if (partsGUI != null) partsGUI.GetComponent<CarPartsIndicator>().resetIndicator();
        }
    }
    public void startGameOver()
    {
        canMove = false;
    }

    public void startGame()
    {
        canMove = true;
    }

    public float getSpeedInput()
    {
        return speedInput;
    }

    public Color getTouchingColor()
    {
        return touchingColor;
    }
}
