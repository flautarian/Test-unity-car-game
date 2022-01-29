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

    public float timeSentinelRaycast;

    public float comboStunt;

    internal Player player;

    internal CarController carController;
    
    private ShopHat hat;

    internal ShopWheel wheel;

    private int actualCarEquipped = 0;
    [SerializeField]
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
    private ParticleSystem turnedUpParticle;

    private AudioSource audioSource;

    private Vector3 targetCorrectTurn = new Vector3(0f ,0f ,0f);

    public float zAngle =0, xAngle =0;

    private void Awake(){
        player = GetComponentInChildren<Player>();
        carController = GetComponent<CarController>();
        if (player != null)
        {
            UpdateActualChosenCar();
            UpdateCarWheels();
            //UpdateCarHat();
            UpdatePlayerControllerDataWithPlayerObject();
        }
        timeSentinelRaycast = Time.time;
        /*stuntComboPSEmissionVar = stuntComboPS.emission;
        stuntComboPSEmissionVar.enabled = false;*/
    }

    void Start()
    {
        if(GlobalVariables.Instance.lastVisitedBuildingPositionPlayer != Vector3.zero &&
            GlobalVariables.Instance.IsWorldMenuGameState())
            TranslatePlayerCar(GlobalVariables.Instance.lastVisitedBuildingPositionPlayer);
        player.gameObject.transform.parent = null;
        Physics.IgnoreLayerCollision(0, 9);
        
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
    }

    private void FixedUpdate()
    {
        //Refresc de posició
        transform.position = player.gameObject.transform.position;

        if(Time.time - timeSentinelRaycast >= 0.2f){

            if(!GlobalVariables.Instance.turnedCar && turned){
                playerAnimator.SetTrigger(Constants.ANIMATION_TRIGGER_TURN_UP);
            }
            // Deteccio canvi de cotxe
            if(actualCarEquipped != GlobalVariables.Instance.GetEquippedCarIndex()){
                UpdateActualChosenCar();
                UpdatePlayerControllerDataWithPlayerObject();
                GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_SMOKE_HIT, this.transform.position);
            }
            // Deteccio canvi de rodes
            else if(wheel.keyCode != GlobalVariables.Instance.GetEquippedWheelIndex()){
                UpdateCarWheels();
                GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_SMOKE_HIT, this.transform.position);
            }
            //TODO: hacer hats 
            //else if(hat.keyCode != GlobalVariables.Instance.GetEquippedHatIndex()){
            //    UpdateCarHat();
            //}
            timeSentinelRaycast = Time.time;
        }
        // update engine motor pitch
        player.UpdatePitchEngine(GetVerticalAxis(), GetHorizontalAxis());
        ManageNitro();
    }

    public void OnCollisionEnter(Collision collision)
    {
        communicatePlayerBaseCollition(collision);
    }
	private void ManageNitro()
    {
        // nitro flag detect control
        if (GlobalVariables.Instance.nitroflag)
        {
            GlobalVariables.Instance.nitroflag = false;
            GlobalVariables.Instance.GetAndPlayChunk(Constants.CHUNK_HIT_NITRO, 1.0f);
            playerAnimator.SetBool(Constants.ANIMATION_NAME_NITRO_BOOL, true);
            StartCoroutine(carController.initializeNitro());
        }
        // Radial blur shader control
        if(playerAnimator.GetBool(Constants.ANIMATION_NAME_NITRO_BOOL))
        	GlobalVariables.Instance.currentRadialBlur += 0.01f;
		else if(GlobalVariables.Instance.currentRadialBlur > 0f)
			GlobalVariables.Instance.currentRadialBlur -= 0.02f;
		
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

    private float CaptureDirectionalKeys(float StartingPoint, KeyCode positive, KeyCode negative){
        if(!Input.GetKey(positive) && !Input.GetKey(negative)){
            if(Math.Abs(StartingPoint) < 0.05) StartingPoint = 0;
            else StartingPoint = Mathf.Lerp(0f, StartingPoint, 25 * Time.deltaTime);
        }
        else{
            StartingPoint += Input.GetKey(positive) ? (StartingPoint < 0f ? 0.1f : 0.05f) : 0.0f;
            StartingPoint += Input.GetKey(negative) ? (StartingPoint > 0f ? -0.1f : -0.05f) : 0.0f;
        }
        return Mathf.Clamp(StartingPoint, -1f, 1f);
    }

    internal void communicateStuntKeyPressed(int keyCode){
        if(trickMode && !playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL)){
            guiController.communicateNewStuntKeyPressed(keyCode, grounded);
        }
    }

    internal float GetVerticalAxis(){
        return carController.VerticalAxis;
    }

    internal float GetHorizontalAxis(){
        return carController.HorizontalAxis;
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
        //stuntComboPSEmissionVar.enabled = true;
        if(stuntComboIndicator != null)
            stuntComboIndicator.AddComboLevel();

        switch(stunt.comboKeys.Count){
            case 2:
                comboStunt = 500f;
            break;
            case 3:
                comboStunt = 1000f;
            break;
            case 4:
                comboStunt = 1500f;
            break;
            case 5:
                comboStunt = 2000f;
            break;
            default:
            break;
        }
        return true;
    }

    internal void impulseUpCar(float amount){
        carController.rBody.AddForce(Vector3.up * amount, ForceMode.Impulse);
    }
    internal void impulseRightCar(float amount){
        carController.rBody.AddForce(Vector3.right * amount, ForceMode.Impulse);
    }

    internal void impulseForwardCar(float amount){
        carController.rBody.AddForce(Vector3.forward * amount, ForceMode.Impulse);
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
                //slowedVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.STICKY_GRASS);
                //wetVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.WET_GRASS);
                streetType = StreetType.grass;
            }
            else if (System.Object.Equals(collision.gameObject.tag, Constants.ASPHALT) && streetType != StreetType.asphalt){
                //slowedVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.STICKY_ROAD);
                //wetVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.WET_ROAD);
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
        foreach (PlayerDestructablePart part in player.parts)
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
                //stuntComboPSEmissionVar.enabled = false;
                comboStunt = 0f;
                if(stuntComboIndicator != null)
                    stuntComboIndicator.ResetComboIndicator();
                if (obstacle.lethal) destroyPlayer(Constants.GAME_OVER_LETHAL_OBS_COLLIDED);
                else
                {
                    if (partDestroyed == null)
                        partDestroyed = findPartNotDestroyed();
                    if (partDestroyed != null){
                        int indexPartToDestroy = player.parts.IndexOf(partDestroyed);
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
        foreach (PlayerDestructablePart dp in player.parts)
        {
            if (dp.destroyed) dp.Recover();
        }
    }

    internal int GetDestructablePartsCount(){
        return player.parts.Count;
    }

    public void startGameOver()
    {
        canMove = false;
    }

    public void startGame()
    {
        canMove = true;
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
        player.gameObject.transform.position = v;
    }
    
    public void UpdateActualChosenCar(){
        GameObject go = GlobalVariables.Instance.LoadActualPlayerCar();
        CarInfo newPlayerObject = go.GetComponent<CarInfo>();
        if(newPlayerObject != null){
            player.UpdatePlayerCarInformation(newPlayerObject);
            actualCarEquipped = GlobalVariables.Instance.GetEquippedCarIndex();
            player.PlayRunningCarChunk();
        }
    }

    internal void UpdateCarWheels(){
        
        GameObject wheelGO = GlobalVariables.Instance.LoadActualPlayerWheel();
        if(wheelGO.TryGetComponent( out ShopWheel w)){
            wheel = w;
        }
    }

    internal void UpdateCarHat(){
        GameObject hatGO = GlobalVariables.Instance.LoadActualPlayerHat();
        if(hatGO.TryGetComponent( out ShopHat h)){
            hat = h;
        }
    }

    public void GenerateMoveParticleEffect(){
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
    }
    public void ResetPlayerControllerRotation(){
        var rot = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);
        transform.rotation = rot;
    }

    private void UpdatePlayerControllerDataWithPlayerObject(){
        playerAnimator.speed = player.stuntHability;
    }

    private void ExecuteTurnUpCar(){
        playerAnimator.SetBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, true);
        var newRot = new Quaternion(transform.rotation.x, transform.rotation.y, 180, transform.rotation.w);
        transform.rotation = newRot;
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_TURNUPCAR, transform.position);
    }
}
