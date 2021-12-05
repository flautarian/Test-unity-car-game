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

    public GUIController guiPlayer;

    public bool grounded = false, canMove = false, turned = false;

    public Vector3 lastSecurePositionPlayer;

    public float turnZAxisEffect = 0;

    public float forwardAccel, normalForwardAccel, reverseAccel, turnStrength, maxWheelTurn;

    public float gravityForce, dragGroundValue;

    public LayerMask whatIsGround;

    RaycastHit hitRayCast;

    public float timeSentinelRaycast;

    public Transform groundRayPoint;

    public float groundRayLength;

    public Rigidbody playerSphereRigidBody;

    public BoxCollider playerBoxCollider;

    internal List<PlayerDestructablePart> destructableParts;

    public float speedInput, comboStunt;

    public float VerticalAxis, HorizontalAxis;

    private Player player;

    private Animator playerAnimator;

    private StuntAnimationOverriderController stuntAnimationOverriderController;

    private GUIController guiController;

    private bool trickMode = false;

    private int isStunting = -1;

    [SerializeField]
    private ParticleSystem stuntComboPS;

    private ParticleSystem.EmissionModule stuntComboPSEmissionVar;

    [SerializeField]
    private ParticleSystem slowedVelocityPS;

    private ParticleSystem.EmissionModule slowedVelocityPSEmissionVar;

    [SerializeField]
    private ParticleSystem wetVelocityPS;

    private ParticleSystem.EmissionModule wetVelocityPSEmissionVar;

    [SerializeField]
    private ParticleSystem turnedUpParticle;

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
        timeSentinelRaycast = Time.time;
        stuntComboPSEmissionVar = stuntComboPS.emission;
        stuntComboPSEmissionVar.enabled = false;
        slowedVelocityPSEmissionVar = slowedVelocityPS.emission;
        slowedVelocityPSEmissionVar.enabled = false;
        wetVelocityPSEmissionVar = wetVelocityPS.emission;
        wetVelocityPSEmissionVar.enabled = false;
    }

    void Start()
    {
        playerSphereRigidBody.transform.parent = null;
        Physics.IgnoreLayerCollision(0, 9);
        // Adapting playerController to the car type chosen
        playerAnimator = GetComponent<Animator>();
        stuntAnimationOverriderController = GetComponent<StuntAnimationOverriderController>();
        GameObject gui = GameObject.FindGameObjectWithTag(Constants.GO_TAG_GUI);
        if (gui != null) guiController = gui.GetComponent<GUIController>();
    }

    private void Update() {
        //Activem l'animacio de 'vehicle xocat' en cas de haver xocat amb el vehicle
        if (playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL) && !playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = true;
        else if (!playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL) && playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = false;

        // apliquem a variable velocitat
        speedInput = VerticalAxis * forwardAccel * 1000f;
        if (VerticalAxis > 0) speedInput += comboStunt;

        //Refresc de posició
        transform.position = playerSphereRigidBody.transform.position;

        if (canMove && !turned)
        {
            turnZAxisEffect = HorizontalAxis * (grounded && !turned ? 5 : 1);
            turnZAxisEffect = Mathf.Clamp(turnZAxisEffect, -5f, 5f);
            if(!IsInStuntMode())
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, HorizontalAxis * turnStrength * Time.deltaTime * VerticalAxis, turnZAxisEffect));
            // manipulacio de shader de radial blur en cas de potenciador de velocitat
            manageNitro();
        }
    }

    internal void UpdatePlayerAnimationStuntMode(bool newState){
        playerAnimator.SetBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, newState);
        trickMode = newState;
    }

    internal bool IsInStuntMode(){
        return isStunting > -1 && trickMode;
    }
    
    internal void updateTrickState(int state){
        isStunting = state;
    }
    

    private void FixedUpdate()
    {
        //Captura de tecles
        HorizontalAxis = CaptureDirectionalKeys(HorizontalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT));
        VerticalAxis = CaptureDirectionalKeys(VerticalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_ACCELERATE), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_DOWN));
        
        if(Time.time - timeSentinelRaycast >= 0.2f){
            // Mirar raycast per posar cotxe paralel al terreny que trepitja i detectem si esta en l'aire o no
            var zAngle = Math.Abs(360- player.transform.rotation.eulerAngles.z);
            if (Physics.Raycast(groundRayPoint.position, -transform.up, out hitRayCast, groundRayLength, whatIsGround)){
                turned = false;
                if(turnedUpParticle.gameObject.activeSelf) 
                    turnedUpParticle.gameObject.SetActive(turned);
                if(!grounded)
                    GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
                grounded = true;
            }
            else if( grounded && (zAngle > 50 && zAngle < 310)){
                if(!turned) {
                    GlobalVariables.Instance.turnedCar = true;
                    turnedUpParticle.gameObject.SetActive(turned);
                }
                turned = true;
                //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, HorizontalAxis * turnStrength * Time.deltaTime * VerticalAxis, 180));
            }
            else {
                grounded = false;
                turned = false;
            }

            timeSentinelRaycast = Time.time;
            if(!GlobalVariables.Instance.turnedCar && turned){
                playerAnimator.SetTrigger(Constants.ANIMATION_TRIGGER_TURN_UP);
            }
        }

        // Adaptem la rotacio del vehicle al terreny
        transform.rotation = Quaternion.FromToRotation(transform.up, hitRayCast.normal) * transform.rotation;

        // apliquem velocitat de rigidbody i gravetat depenent del estat del cotxe
        if(canMove && !turned){
            if (grounded)
            {
                if(streetType == StreetType.asphalt) lastSecurePositionPlayer = transform.position;
                playerSphereRigidBody.drag = dragGroundValue;
                if (Math.Abs(speedInput) > 0 && VerticalAxis != 0) playerSphereRigidBody.AddForce(transform.forward * speedInput);
            }
            else
            {
                playerSphereRigidBody.drag = 0.1f;
                playerSphereRigidBody.AddForce(Vector3.up * -gravityForce * 100f);
            }
        }
        else if (!canMove || turned) playerSphereRigidBody.drag = 3.5f;
    }

    private float CaptureDirectionalKeys(float StartingPoint, KeyCode positive, KeyCode negative){
        var localHSensibility = GlobalVariables.Instance.GetHSensibilityLevel();
        if(!Input.GetKey(positive) && !Input.GetKey(negative))
            if(Math.Abs(StartingPoint) < localHSensibility) StartingPoint = 0;
            else StartingPoint = Mathf.Lerp(0f, StartingPoint, 10*Time.deltaTime);
        else{
            StartingPoint += Input.GetKey(positive) ? localHSensibility : 0.0f;
            StartingPoint += Input.GetKey(negative) ? -localHSensibility : 0.0f;
        }
        return Mathf.Clamp(StartingPoint, -1f, 1f);
    }

    internal void communicateStuntKeyPressed(int keyCode){
        if(trickMode && !playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL)){
            guiController.communicateNewStuntKeyPressed(keyCode, grounded);
        }
    }

    internal void communicateStuntInitialized(){
        if(guiController != null)
            guiController.communicateStuntInitialized();
    }

    internal void communicateStuntClose(){
        if(guiController != null)
            guiController.communicateStuntClose();
    }

    internal void communicateStuntReset(){
        if(guiController != null)
            guiController.communicateStuntReset();
    }

    internal void InitStunt(Stunt stunt){
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_HIT, transform.position);
        stuntAnimationOverriderController.Set(stunt.GetAnimation());
        playerAnimator.SetTrigger(Constants.ANIMATION_TRIGGER_INIT_STUNT);
        playerAnimator.SetBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, true);
        stuntComboPSEmissionVar.enabled = true;
        switch(stunt.comboKeys.Count){
            case 2:
                GlobalVariables.Instance.addStuntEC(5);
                if(stuntComboPSEmissionVar.rateOverTime.constant < 5.0f){
                    stuntComboPSEmissionVar.rateOverTime = 5.0f;
                }
                comboStunt = 500f;
            break;
            case 3:
                GlobalVariables.Instance.addStuntEC(10);
                if(stuntComboPSEmissionVar.rateOverTime.constant < 10.0f){
                    stuntComboPSEmissionVar.rateOverTime = 10.0f;
                }
                comboStunt = 1000f;
            break;
            case 4:
                if(stuntComboPSEmissionVar.rateOverTime.constant < 15.0f){
                    stuntComboPSEmissionVar.rateOverTime = 15.0f;
                }
                comboStunt = 1500f;
            break;
            case 5:
                if(stuntComboPSEmissionVar.rateOverTime.constant < 20.0f){
                    stuntComboPSEmissionVar.rateOverTime = 20.0f;
                }
                comboStunt = 2000f;
            break;
            default:
            break;
        }
    }

    internal void impulseUpCar(float amount){
        playerSphereRigidBody.AddForce(Vector3.up * amount, ForceMode.Impulse);
    }
    internal void impulseRightCar(float amount){
        playerSphereRigidBody.AddForce(Vector3.right * amount, ForceMode.Impulse);
    }

    internal void turnLeft()
    {
        throw new NotImplementedException();
    }

    internal void turnRight()
    {
        throw new NotImplementedException();
    }

    public void SphereEnterCollides(Collision collision)
    {
        if (System.Object.Equals(collision.gameObject.layer, 8))// Ground
        {
            grounded = true;
            if (System.Object.Equals(collision.gameObject.tag, Constants.CESPED) && streetType != StreetType.grass){
                slowedVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.STICKY_GRASS);
                wetVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.WET_GRASS);
                Debug.Log("detected grass: " + wetVelocityPSEmissionVar.enabled);
                streetType = StreetType.grass;
            }
            else if (System.Object.Equals(collision.gameObject.tag, Constants.ASPHALT) && streetType != StreetType.asphalt){
                slowedVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.STICKY_ROAD);
                wetVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.WET_ROAD);
                Debug.Log("detected asphalt: " + wetVelocityPSEmissionVar.enabled);
                streetType = StreetType.asphalt;
            }
            else if (System.Object.Equals(collision.gameObject.tag, Constants.WATER))
            {
                streetType = StreetType.water;
                destroyPlayer(Constants.GAME_OVER_VEHICLE_DROWNED);
            }
        }
    }
    private void destroyPlayer(string reason)
    {
        if(GlobalVariables.Instance.gameMode == GameMode.WOLRDMAINMENU){
            StartCoroutine(ResetPosition());
        }
        else{
            playerAnimator.SetBool(Constants.ANIMATION_NAME_EXPLODE_BOOL, true);
            guiController.startGameOver(reason);
        }
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
        else if(collision.gameObject.tag.Contains(Constants.GO_TAG_CONTAINS_GOALLINE))
            guiController.StartGameWon();
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
        if(obstacle != null){
            if(GlobalVariables.Instance.gameMode == GameMode.WOLRDMAINMENU) {
                StartCoroutine(obstacle.InitializeMainMenuResetPosition());
                return;
            }
            else if (obstacle.penalizableObstacle)
            {
                // conseqüencies de col·licio
                stuntComboPSEmissionVar.enabled = false;
                comboStunt = 0f;
                if (obstacle.lethal) destroyPlayer(Constants.GAME_OVER_LETHAL_OBS_COLLIDED);
                else
                {
                    if (partDestroyed == null)
                        partDestroyed = findPartNotDestroyed();
                    if (partDestroyed != null)
                    {
                        int indexPartToDestroy = destructableParts.IndexOf(partDestroyed);

                        if (!playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL))
                        {
                            if(trickMode) communicateStuntReset();
                            // executing particles from GlobalVariables
                            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_BOOM, partDestroyed.transform.position);
                            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_HIT, partDestroyed.transform.position);
                            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_SMOKE_HIT, partDestroyed.transform.position);
                            // shader effects
                            GlobalVariables.Instance.currentBrokenScreen = 0.05f * (indexPartToDestroy + 1);
                            GlobalVariables.Instance.shakeParam += 2.5f;
                            // si esta en mode stunt cancelar·lo perque el cotxe ha xocat
                            if(trickMode) UpdatePlayerAnimationStuntMode(false);
                            // habilitant boolea de col·licio de cotxe
                            playerAnimator.SetBool(Constants.ANIMATION_NAME_HIT_BOOL, true);
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
        if (GlobalVariables.Instance.nitroflag)
        {
            forwardAccel = normalForwardAccel;
            GlobalVariables.Instance.currentRadialBlur = 0;
            StartCoroutine(initializeNitro());
        }
        else if(playerAnimator.GetBool(Constants.ANIMATION_NAME_NITRO_BOOL))
        {
            float valRadial = GlobalVariables.Instance.currentRadialBlur;
            if (forwardAccel > normalForwardAccel)
                valRadial += 0.01f;
            else
            {
                if (valRadial > 0)
                    valRadial -= 0.02f;
                else {
                    valRadial = 0;
                    playerAnimator.SetBool(Constants.ANIMATION_NAME_NITRO_BOOL, false);
                }
            }
            GlobalVariables.Instance.currentRadialBlur = valRadial;
        }
    }

    private IEnumerator initializeNitro()
    {
        playerAnimator.SetBool(Constants.ANIMATION_NAME_NITRO_BOOL, true);
        GlobalVariables.Instance.nitroflag = false;
        forwardAccel += 2;
        yield return new WaitForSeconds(3f);
        forwardAccel -= 2;
    }

    public IEnumerator ResetPosition(){
        var panelCanvas = GameObject.FindGameObjectWithTag(Constants.GO_TAG_PANEL_CANVAS_CONTAINER);
        Animator animator = null;
        if(panelCanvas != null)
            animator = panelCanvas.GetComponent<Animator>();
        if(animator != null){
            animator.SetTrigger("ResetPlayerPosition");
            yield return new WaitForSeconds(1f);
            playerSphereRigidBody.gameObject.transform.position = lastSecurePositionPlayer;
            transform.position = lastSecurePositionPlayer;
        }
    }

    public void GenerateMoveParticleEffect(){
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
    }
    public void ResetPlayerControllerRotation(){
        var rot = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);
        transform.rotation = rot;
    }
}
