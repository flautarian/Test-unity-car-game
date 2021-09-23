﻿using Assets.Scripts;
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

    public GUIController guiPlayer;

    public bool grounded = false, canMove = false;

    public float turnZAxisEffect = 0;

    public float forwardAccel, normalForwardAccel, reverseAccel, turnStrength, maxWheelTurn;

    public float gravityForce, dragGroundValue;

    public LayerMask whatIsGround;

    RaycastHit hitRayCast;

    public Transform groundRayPoint;

    public float groundRayLength;

    public Rigidbody playerSphereRigidBody;

    public BoxCollider playerBoxCollider;

    internal List<PlayerDestructablePart> destructableParts;

    public float speedInput;

    public float VerticalAxis, HorizontalAxis;

    private Player player;

    private Animator playerAnimator;

    private GUIController guiController;

    private bool trickMode = false;

    private int isTricking = -1;

    private void Awake(){
        player = GetComponentInChildren<Player>();
        if (player != null)
        {
            gravityForce = player.gravityForce;
            dragGroundValue = player.dragGroundForce;
            forwardAccel = player.forwardAccel;
            normalForwardAccel = forwardAccel;
            reverseAccel = player.reverseAccel;
            turnStrength = player.turnStrength;
            maxWheelTurn = player.maxWheelTurn;
            destructableParts = player.parts;
        }
    }

    void Start()
    {
        playerSphereRigidBody.transform.parent = null;
        Physics.IgnoreLayerCollision(0, 9);
        // Adapting playerController to the car type chosen
        playerAnimator = GetComponent<Animator>();
        GameObject gui = GameObject.FindGameObjectWithTag(Constants.GO_TAG_GUI);
        if (gui != null) guiController = gui.GetComponent<GUIController>();
    }

    private void Update() {

        //Activem l'animacio de 'vehicle xocat' en cas de haver xocat amb el vehicle
        if (playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL) && !playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = true;
        else if (!playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL) && playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = false;

        //Captura de tecles
        VerticalAxis = Input.GetAxis(Constants.INPUT_ACCELERATE);
        HorizontalAxis = Input.GetAxis(Constants.AXIS_HORIZONTAL);

        // apliquem a variable velocitat
        if (VerticalAxis > 0) speedInput = VerticalAxis * forwardAccel * 1000f;

        //Refresc de posició
        transform.position = playerSphereRigidBody.transform.position;

        if (canMove)
        {
            turnZAxisEffect = HorizontalAxis * (grounded ? 5 : 1);
            turnZAxisEffect = Mathf.Clamp(turnZAxisEffect, -5f, 5f);
            if(!IsInStuntMode())transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, HorizontalAxis * turnStrength * Time.deltaTime * VerticalAxis, turnZAxisEffect));
            // manipulacio de shader de radial blur en cas de potenciador de velocitat
            manageNitro();
        }
    }

    internal void UpdatePlayerAnimationStuntMode(bool newState){
        playerAnimator.SetBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, newState);
        trickMode = newState;
    }

    internal bool IsInStuntMode(){
        return isTricking > -1 && trickMode;
    }
    
    internal void updateTrickState(int state){
        isTricking = state;
    }
    

    private void FixedUpdate()
    {
        // Mire raycast per posar cotxe paralel al terreny que trepitja i detectem si esta en l'aire o no
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hitRayCast, groundRayLength, whatIsGround))
        {
            if (!grounded) GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
            grounded = true;
        }
        else grounded = false;

        // Adaptem la rotacio del vehicle al terreny
        transform.rotation = Quaternion.FromToRotation(transform.up, hitRayCast.normal) * transform.rotation;

        // apliquem velocitat de rigidbody i gravetat depenent del estat del cotxe
        if (grounded && canMove)
        {
            playerSphereRigidBody.drag = dragGroundValue;
            if (Math.Abs(speedInput) > 0 && VerticalAxis != 0) playerSphereRigidBody.AddForce(transform.forward * speedInput);
        }
        else
        {
            playerSphereRigidBody.drag = 0.1f;
            playerSphereRigidBody.AddForce(Vector3.up * -gravityForce * 100f);
        }
        //ManageTricks();
        if (!canMove) playerSphereRigidBody.drag = 3.5f;

    }

    internal void communicateStuntKeyPressed(int keyCode){
        if(trickMode && !playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL)){
            guiController.communicateNewStuntKeyPressed(keyCode, grounded);
        }
    }

    internal void communicateStuntInitialized(){
        guiController.communicateStuntInitialized();
    }

    internal void communicateStuntClose(){
        guiController.communicateStuntClose();
    }

    internal void communicateStuntReset(){
        guiController.communicateStuntReset();
    }

    internal void InitStunt(int stunt){
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_HIT, transform.position);
        playerAnimator.SetInteger(Constants.ANIMATION_NAME_CAST_STUNT_INT, stunt);
    }

    internal void turnLeft()
    {
        throw new NotImplementedException();
    }

    internal void turnRight()
    {
        throw new NotImplementedException();
    }


    private void ManageTricks()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 1.0f * Time.deltaTime);
    }

    public void SphereEnterCollides(Collision collision)
    {
        if (System.Object.Equals(collision.gameObject.layer, 8))// Ground
        {
            if (System.Object.Equals(collision.gameObject.tag, Constants.CESPED))
                streetType = StreetType.grass;
            else if (System.Object.Equals(collision.gameObject.tag, Constants.ASPHALT))
                streetType = StreetType.asphalt;
            else if (System.Object.Equals(collision.gameObject.tag, Constants.WATER))
            {
                streetType = StreetType.water;
                destroyPlayer(Constants.GAME_OVER_VEHICLE_DROWNED);
            }

        }
    }
    private void destroyPlayer(string reason)
    {
        playerAnimator.SetBool(Constants.ANIMATION_NAME_EXPLODE_BOOL, true);
        guiController.startGameOver(reason);
    }

    public void executeCarExplosionParticle()
    {
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_BOOM, transform.position);
    }

    internal void AddCoins(int number)
    {
        GlobalVariables.Instance.addCoins(number);
    }

    internal void communicatePlayerBaseCollition(Collision collision)
    {
        if (collision.gameObject.tag.Contains(Constants.GO_TAG_CONTAINS_OBSTACULO))
            ComunicateCollisionPart(null, collision.collider);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawCube(com, new Vector3(0.25f, 0.25f, 0.25f));
    }

    private PlayerDestructablePart findPartNotDestroyed()
    {
        foreach (PlayerDestructablePart part in destructableParts)
        {
            if (!part.destroyed) return part;
        }
        return null;
    }
    internal void ComunicateCollisionPart(PlayerDestructablePart partDestroyed, Collider collision)
    {
        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle != null && obstacle.penalizableObstacle)
        {
            if (obstacle.lethal) destroyPlayer(Constants.GAME_OVER_LETHAL_OBS_COLLIDED);
            else
            {
                if (partDestroyed == null)
                    partDestroyed = findPartNotDestroyed();
                if (partDestroyed != null)
                {
                    int indexPartToDestroy = destructableParts.IndexOf(partDestroyed);

                    // car conseqüences
                    if (!playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL))
                    {
                        if(trickMode) communicateStuntReset();
                        // explosion particle init
                        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_BOOM, partDestroyed.transform.position);
                        // shader effects
                        GlobalVariables.Instance.currentBrokenScreen = 0.05f * (indexPartToDestroy + 1);
                        GlobalVariables.Instance.shakeParam += 2.5f;
                        // enabling animation pass to animator Player
                        if(trickMode) UpdatePlayerAnimationStuntMode(false);
                        playerAnimator.SetBool(Constants.ANIMATION_NAME_HIT_BOOL, true);
                        // executing particles from GlobalVariables
                        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_HIT, partDestroyed.transform.position);
                        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_SMOKE_HIT, partDestroyed.transform.position);
                        if (guiPlayer != null && guiPlayer.carPartsIndicator != null)
                            guiPlayer.carPartsIndicator.decrementPart();
                        partDestroyed.ejectPart();
                        //collision.gameObject.GetComponent<Obstacle>().Collide(partDestroyed.transform);
                        partDestroyed.Inhabilite();
                    }
                }
                else
                {
                    destroyPlayer(Constants.GAME_OVER_VEHICLE_DESTROYED);
                }
            }
        }
        else
        {
            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
        }
    }

    internal void RecoverParts()
    {
        foreach (PlayerDestructablePart dp in destructableParts)
        {
            if (dp.destroyed)
                dp.Recover();
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

    private void manageNitro()
    {
        GlobalVariables.Instance.playerCurrentVelocity = playerSphereRigidBody.velocity.z;
        if (!playerAnimator.GetBool(Constants.ANIMATION_NAME_NITRO_BOOL))
        {
            forwardAccel = normalForwardAccel;
            GlobalVariables.Instance.currentRadialBlur = 0;
            if (GlobalVariables.Instance.nitroflag)
            {
                StartCoroutine(initializeNitro());
            }
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

    private IEnumerator initializeNitro()
    {
        forwardAccel += 2;
        yield return new WaitForSeconds(3f);
        forwardAccel -= 2;
        GlobalVariables.Instance.nitroflag = false;
    }
}