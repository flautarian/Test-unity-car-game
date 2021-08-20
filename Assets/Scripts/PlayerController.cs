using Assets.Scripts;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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

    public float forwardAccel, normalForwardAccel, reverseAccel, turnStrength, maxWheelTurn;

    public float gravityForce, dragGroundValue;

    public LayerMask whatIsGround;

    public Transform groundRayPoint;

    public Rigidbody playerSphereRigidBody;

    public BoxCollider playerBoxCollider;

    public List<GameObject> destructableParts = new List<GameObject>();

    private Color touchingColor;

    private float speedInput;

    public float VerticalAxis, HorizontalAxis;

    private Player player;

    private Animator playerAnimator;

    void Start()
    {
        playerSphereRigidBody.transform.parent = null;
        Physics.IgnoreLayerCollision(0, 9);
        player = GetComponentInChildren<Player>();
        // Adapting playerController to the car type chosen
        if(player != null)
        {
            forwardAccel = player.forwardAccel;
            normalForwardAccel = forwardAccel;
            reverseAccel = player.reverseAccel;
            turnStrength = player.turnStrength;
            maxWheelTurn = player.maxWheelTurn;
        }
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Activem l'animacio de 'vehicle xocat' en cas de haver xocat amb el vehicle
        if (playerAnimator.GetBool("hit") && !playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = true;
        else if (!playerAnimator.GetBool("hit") && playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = false;

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
            // manipulacio de shader de radial blur en cas de potenciador de velocitat
            manageNitro();
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
        playerAnimator.SetBool("explode", true);
        GameObject gui = GameObject.FindGameObjectWithTag("GUI");
        if (gui != null) gui.GetComponent<GUIController>().startGameOver(reason);
    }

    internal void AddCoins(int number)
    {
        GlobalVariables.Instance.addCoins(number);
    }

    internal void communicatePlayerBaseCollition(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Obstaculo"))
            ComunicateCollisionPart(null, collision.collider);
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
        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle != null && obstacle.penalizableObstacle)
        {
            if (obstacle.lethal) destroyPlayer("collitioned with lethal object");
            else
            {
                if (partDestroyed == null) partDestroyed = findPartNotDestroyed();
                if (partDestroyed != null)
                {
                    int indexPartToDestroy = destructableParts.IndexOf(partDestroyed);

                    // car conseqüences
                    if (!playerAnimator.GetBool("hit"))
                    {
                        // shader effects
                        GlobalVariables.Instance.currentBrokenScreen = 0.05f * (indexPartToDestroy + 1);
                        GlobalVariables.Instance.shakeParam += 2.5f;
                        playerAnimator.SetBool("hit", true);
                        hitParticle.transform.position = partDestroyed.transform.position;
                        smokeHitParticle.transform.position = partDestroyed.transform.position;
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
                else
                {
                    destroyPlayer("Vehicle destroyed");
                }
            }
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

    internal void AddNitro()
    {
        playerAnimator.SetBool("nitro", true);
    }
    private void manageNitro()
    {
        if (!playerAnimator.GetBool("nitro"))
        {
            forwardAccel = normalForwardAccel;
            GlobalVariables.Instance.currentRadialBlur = 0;
        }
        else
        {
            float valRadial = GlobalVariables.Instance.currentRadialBlur;
            if (forwardAccel > normalForwardAccel)
                valRadial += 0.01f;
            else
            {
                if (valRadial > 0)
                    valRadial -= 0.03f;
                else valRadial = 0;
            }
            GlobalVariables.Instance.currentRadialBlur = valRadial;
        }
    }
}
