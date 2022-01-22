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
    public GUIController guiPlayer;

    public bool grounded = false, canMove = false, turned = false;

    public Vector3 lastSecurePositionPlayer;

    public float turnZAxisEffect = 0;

    public float forwardAccel, normalForwardAccel, reverseAccel, turnStrength, maxWheelTurn, accel;

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

    private int actualCarEquipped = 0;

    private Animator playerAnimator;

    private StuntAnimationOverriderController stuntAnimationOverriderController;

    private GUIController guiController;

    private bool trickMode = false;

    private int isStunting = -1;

    public StuntType actualStuntTricking = StuntType.NONE;

    [SerializeField]
    private ParticleSystem stuntComboPS;

    [SerializeField]
    private StuntComboIndicator stuntComboIndicator;

    private ParticleSystem.EmissionModule stuntComboPSEmissionVar;

    [SerializeField]
    private ParticleSystem slowedVelocityPS;

    private ParticleSystem.EmissionModule slowedVelocityPSEmissionVar;

    [SerializeField]
    private ParticleSystem wetVelocityPS;

    private ParticleSystem.EmissionModule wetVelocityPSEmissionVar;

    [SerializeField]
    private ParticleSystem turnedUpParticle;

    private AudioSource audioSource;

    private Quaternion targetCorrectRotation;

    private Vector3 targetCorrectTurn = new Vector3(0f ,0f ,0f);

    public float zAngle =0, xAngle =0;

    private void Awake(){
        player = GetComponentInChildren<Player>();
        if (player != null)
        {
            UpdateActualChosenCar();
            UpdatePlayerControllerDataWithPlayerObject();
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
        if(GlobalVariables.Instance.lastVisitedBuildingPositionPlayer != Vector3.zero &&
            GlobalVariables.Instance.IsWorldMenuGameState())
            TranslatePlayerCar(GlobalVariables.Instance.lastVisitedBuildingPositionPlayer);
        playerSphereRigidBody.transform.parent = null;
        Physics.IgnoreLayerCollision(0, 9);
        // Adapting playerController to the car type chosen
        playerAnimator = GetComponent<Animator>();
        playerAnimator.speed = player.stuntHability;
        stuntAnimationOverriderController = GetComponent<StuntAnimationOverriderController>();
        audioSource = GetComponent<AudioSource>();
        GameObject gui = GameObject.FindGameObjectWithTag(Constants.GO_TAG_GUI);
        if (gui != null) guiController = gui.GetComponent<GUIController>();
    }
    private void Update() {
        //Activem l'animacio de 'vehicle xocat' en cas de haver xocat amb el vehicle
        /*if(playerBoxCollider != null){
            if (playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL) && !playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = true;
            else if (!playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL) && playerBoxCollider.isTrigger) playerBoxCollider.isTrigger = false;
        }*/

        // apliquem a variable velocitat
        speedInput = Mathf.Lerp(speedInput, (VerticalAxis * forwardAccel * 1000f) + comboStunt, Time.deltaTime * accel);

        //Refresc de posició
        transform.position = playerSphereRigidBody.transform.position;
    }

    private void FixedUpdate()
    {
        if (canMove && !turned)
        {
            turnZAxisEffect = HorizontalAxis * (grounded && !turned ? 5 : 1);
            turnZAxisEffect = Mathf.Clamp(turnZAxisEffect, -5f, 5f);
            // manipulacio de shader de radial blur en cas de potenciador de velocitat
            manageNitro();
        }
        
        if(GlobalVariables.Instance.playerTargetedByCamera && canMove){
            //Captura de tecles
            HorizontalAxis = CaptureDirectionalKeys(HorizontalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT));
            VerticalAxis = CaptureDirectionalKeys(VerticalAxis, GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_ACCELERATE), GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_DOWN));
        }
        else{
            HorizontalAxis =0;
            VerticalAxis = 0;
        }

        // Mirar raycast per posar cotxe paralel al terreny que trepitja i detectem si esta en l'aire o no
        ManageTurnUpPlayerOrientation();
        if(Time.time - timeSentinelRaycast >= 0.2f){

            if(!GlobalVariables.Instance.turnedCar && turned){
                playerAnimator.SetTrigger(Constants.ANIMATION_TRIGGER_TURN_UP);
            }

            if(actualCarEquipped != GlobalVariables.Instance.GetEquippedCarIndex()){
                UpdateActualChosenCar();
                UpdatePlayerControllerDataWithPlayerObject();
                GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_SMOKE_HIT, this.transform.position);
            }

            timeSentinelRaycast = Time.time;
        }

        // update engine motor pitch
        player.UpdatePitchEngine(VerticalAxis, HorizontalAxis);

        // Adaptem la rotacio del vehicle al terreny
        if(grounded){
            if(!IsInStuntMode()){
                targetCorrectTurn.y = HorizontalAxis * turnStrength * Time.deltaTime * VerticalAxis;
                targetCorrectTurn.z = turnZAxisEffect;
                targetCorrectRotation = Quaternion.Euler(transform.rotation.eulerAngles + targetCorrectTurn);
            }
            transform.rotation = Quaternion.FromToRotation(transform.up, hitRayCast.normal) * targetCorrectRotation;
        }
        else {
            // posem rotant correctament el vehicle per evitar angles imposibles en l'aire
            targetCorrectRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetCorrectRotation, Time.deltaTime);
        }

        // apliquem velocitat de rigidbody i gravetat depenent del estat del cotxe
        if(canMove && !turned){
            if (grounded)
            {
                playerSphereRigidBody.drag = dragGroundValue;
                if (Math.Abs(speedInput) > 0 && VerticalAxis != 0) {

                    playerSphereRigidBody.AddForce(transform.forward * speedInput);
                }
            }
            else
            {
                playerSphereRigidBody.drag = 0f;
                playerSphereRigidBody.AddForce(Vector3.up * -gravityForce * 100f);
            }
        }
        else if (!canMove || turned) playerSphereRigidBody.drag = 3.5f;
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

    internal void updateActualTrick(StuntType stuntType){
        actualStuntTricking = stuntType;
    }
    

    private void ManageTurnUpPlayerOrientation(){
        zAngle = Math.Abs(360 - transform.rotation.eulerAngles.z);
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hitRayCast, groundRayLength, whatIsGround)){
            turned = false;
            if(turnedUpParticle.gameObject.activeSelf) 
                turnedUpParticle.gameObject.SetActive(turned);
            grounded = true;
        }
        else if( grounded && ((zAngle > 110 && zAngle < 255))){
            if(!turned) {
                GlobalVariables.Instance.turnedCar = true;
                stuntComboIndicator.ResetComboIndicator();
                turnedUpParticle.gameObject.SetActive(turned);
                playerAnimator.SetBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, false);
            }
            turned = true;
        }
        else {
            grounded = false;
            turned = false;
            GlobalVariables.Instance.turnedCar = turned;
            turnedUpParticle.gameObject.SetActive(turned);
        }
            
    }

    private float CaptureDirectionalKeys(float StartingPoint, KeyCode positive, KeyCode negative){
        var localHSensibility = GlobalVariables.Instance.GetHSensibilityLevel();
        if(!Input.GetKey(positive) && !Input.GetKey(negative))
            if(Math.Abs(StartingPoint) < localHSensibility) StartingPoint = 0;
            else StartingPoint = Mathf.Lerp(0f, StartingPoint, 10 * Time.deltaTime);
        else{
            StartingPoint += Input.GetKey(positive) ? 0.05f : 0.0f;
            StartingPoint += Input.GetKey(negative) ? -0.05f : 0.0f;
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

    internal bool InitStunt(Stunt stunt){
        GlobalVariables.Instance.AddObjectivePoint(stunt.groundStunt ? ObjectiveGameType.NUMBER_GROUNDAL_STUNTS : ObjectiveGameType.NUMBER_AERIAL_STUNTS, 1);
        if(stunt.stuntType != StuntType.NORMAL && stunt.units > GlobalVariables.Instance.totalStuntEC)
        {
            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_HIT, transform.position);
            GlobalVariables.Instance.GetAndPlayChunk("UI_Ko", 1.0f);
            return false;
        }
        else if(stunt.stuntType == StuntType.NORMAL)
            GlobalVariables.Instance.addStuntEC(stunt.units);
        else
            GlobalVariables.Instance.substractStuntEC(stunt.units);
        RequestAndPlayChunk(stunt.chunkName);
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_HIT, transform.position);
        stuntAnimationOverriderController.SetAnimation("StuntDefaultAnimation", stunt.GetAnimation());
        playerAnimator.SetTrigger(Constants.ANIMATION_TRIGGER_INIT_STUNT);
        playerAnimator.SetBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, true);
        stuntComboPSEmissionVar.enabled = true;
        stuntComboIndicator.AddComboLevel();

        switch(stunt.comboKeys.Count){
            case 2:
                if(stuntComboPSEmissionVar.rateOverTime.constant < 5.0f){
                    stuntComboPSEmissionVar.rateOverTime = 5.0f;
                }
                comboStunt = 500f;
            break;
            case 3:
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
        return true;
    }

    internal void impulseUpCar(float amount){
        playerSphereRigidBody.AddForce(Vector3.up * amount, ForceMode.Impulse);
    }
    internal void impulseRightCar(float amount){
        playerSphereRigidBody.AddForce(Vector3.right * amount, ForceMode.Impulse);
    }

    internal void impulseForwardCar(float amount){
        playerSphereRigidBody.AddForce(Vector3.forward * amount, ForceMode.Impulse);
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
            if(!grounded){
                GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
                if(!System.Object.Equals(collision.gameObject.tag, Constants.WATER)) lastSecurePositionPlayer = transform.position;
                if(isStunting > -1){
                    ExecuteTurnUpCar();
                }
            }
            grounded = true;
            if (System.Object.Equals(collision.gameObject.tag, Constants.CESPED) && streetType != StreetType.grass){
                slowedVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.STICKY_GRASS);
                wetVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.WET_GRASS);
                streetType = StreetType.grass;
            }
            else if (System.Object.Equals(collision.gameObject.tag, Constants.ASPHALT) && streetType != StreetType.asphalt){
                slowedVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.STICKY_ROAD);
                wetVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.WET_ROAD);
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
        if(turned){
            turned = false;
            GlobalVariables.Instance.turnedCar = turned;
            turnedUpParticle.gameObject.SetActive(turned);
        }
        if(GlobalVariables.Instance.gameMode == GameMode.WOLRDMAINMENU){
            StartCoroutine(ResetPosition());
        }
        else{
            playerAnimator.SetTrigger(Constants.ANIMATION_NAME_EXPLODE_BOOL);
            guiController.startGameOver(reason);
        }
    }

    public void executeCarExplosionParticle()
    {
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_BOOM, transform.position);
    }

    internal void communicatePlayerBaseCollition(Collision collision)
    {
        if (collision.gameObject.tag.Contains(Constants.GO_TAG_CONTAINS_OBSTACULO) && !turned)
            ComunicateCollisionPart(null, collision.collider);
    }

    public void StartGameWon(){
        if(guiController != null)
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
            
            if (obstacle.penalizableObstacle)
            {
                RequestAndPlayChunk(Constants.CHUNK_HIT_PLAYER);
                // conseqüencies de col·licio
                stuntComboPSEmissionVar.enabled = false;
                comboStunt = 0f;
                stuntComboIndicator.ResetComboIndicator();
                if (obstacle.lethal) destroyPlayer(Constants.GAME_OVER_LETHAL_OBS_COLLIDED);
                else
                {
                    if (partDestroyed == null)
                        partDestroyed = findPartNotDestroyed();
                    if (partDestroyed != null){
                        int indexPartToDestroy = destructableParts.IndexOf(partDestroyed);
                        if (!playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL))
                        {
                            stuntAnimationOverriderController.SetAnimation("playerHitAndRecovery", playerAnimator.runtimeAnimatorController.animationClips[0]);
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
                            // ejectant part del cotxe per mostrar danys per colisió
                            if(GlobalVariables.Instance.IsLevelGameState()){
                                if (guiPlayer != null && guiPlayer.carPartsIndicator != null)
                                    guiPlayer.carPartsIndicator.decrementPart();
                                partDestroyed.ejectPart();
                                //collision.gameObject.GetComponent<Obstacle>().Collide(partDestroyed.transform);
                                partDestroyed.Inhabilite();
                            }
                        }
                        else 
                            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
                    }
                    else 
                        destroyPlayer(Constants.GAME_OVER_VEHICLE_DESTROYED);
                    
                    if(GlobalVariables.Instance.IsWorldMenuGameState()) 
                        StartCoroutine(obstacle.InitializeMainMenuResetPosition());
                }
            }
        }
    }

    private void RequestAndPlayChunk(string chunk){
        AudioClip clip = PoolManager.Instance.SpawnChunkFromPool(chunk);
        if(clip != null) audioSource.PlayOneShot(clip);
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
        RequestAndPlayChunk(Constants.CHUNK_HIT_NITRO);
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
            TranslatePlayerCar(lastSecurePositionPlayer);
        }
    }

    private void TranslatePlayerCar(Vector3 v){
        playerSphereRigidBody.gameObject.transform.position = v;
            transform.position = v;
    }
    
    public void UpdateActualChosenCar(){
        GameObject go = GlobalVariables.Instance.LoadActualPlayerCar();
        Player newPlayerObject = go.GetComponent<Player>();
        BoxCollider bc = go.GetComponent<BoxCollider>();
        if(newPlayerObject != null){
            newPlayerObject.transform.parent = this.transform;
            newPlayerObject.transform.position = player.transform.position;
            newPlayerObject.transform.rotation = player.transform.rotation;
            var name = player.transform.name;
            Destroy(player.gameObject);
            player = newPlayerObject;
            player.transform.name = name;
            player.controller = this;
            UpdateSphereScale();
            actualCarEquipped = GlobalVariables.Instance.GetEquippedCarIndex();
            if(bc != null) playerBoxCollider = bc;
            player.PlayRunningCarChunk();
        }
    }
    
    internal void UpdateSphereScale(){
        var newSphereSize = new Vector3(1,1,1);
        newSphereSize.x += player.sphereOffset;
        playerSphereRigidBody.transform.localScale = newSphereSize;
    }

    public void GenerateMoveParticleEffect(){
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
    }
    public void ResetPlayerControllerRotation(){
        var rot = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);
        transform.rotation = rot;
    }

    private void UpdatePlayerControllerDataWithPlayerObject(){
        if(player != null){
            gravityForce = player.gravityForce;
            dragGroundValue = player.dragGroundForce;
            forwardAccel = player.forwardAccel;
            normalForwardAccel = forwardAccel;
            reverseAccel = player.reverseAccel;
            turnStrength = player.turnStrength;
            maxWheelTurn = player.maxWheelTurn;
            accel = player.accel;
            destructableParts = player.parts;
            if( playerAnimator != null )playerAnimator.speed = player.stuntHability;
        }
    }

    private void ExecuteTurnUpCar(){
        playerAnimator.SetBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, true);
        var newRot = new Quaternion(transform.rotation.x, transform.rotation.y, 180, transform.rotation.w);
        transform.rotation = newRot;
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_TURNUPCAR, transform.position);
    }
}
