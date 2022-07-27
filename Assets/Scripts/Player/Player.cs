using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Player : MonoBehaviour
{
    [SerializeField]
    internal StuntsController stuntsController;
    public CarController carController;
    [SerializeField]
    internal List<PlayerDestructablePart> parts;
    [SerializeField]
    internal AudioSource RunningCarAudioSource;
    [SerializeField]
    internal AudioSource SkidCarAudioSource;
    [SerializeField]
    internal AudioClip initCarChunk;
    [SerializeField]
    internal AudioClip runningCarChunk;
    private float timeSentinelRaycast;
    
    private Vector3 lastSecurePositionPlayer = Vector3.zero;

    private int actualCarEquipped = 0;
    [SerializeField]
    internal ShopHat actualHat;
    [SerializeField]
    internal ShopWheel actualWheel;

    [SerializeField]
    private MeshFilter mainMeshFilter;
    
    [SerializeField]
    private ParticleSystem turnedUpParticle;

    private BoxCollider collider;

    private void Awake() {
        SkidCarAudioSource.loop = true;
        RunningCarAudioSource.loop = true;
        timeSentinelRaycast = Time.time;
        collider = GetComponent<BoxCollider>();
    }

    private void Start() {
        if(GlobalVariables.Instance.lastVisitedBuildingPositionPlayer != Vector3.zero &&
            GlobalVariables.Instance.IsWorldMenuGameState())
            TranslatePlayerCar(GlobalVariables.Instance.lastVisitedBuildingPositionPlayer);
        UpdateActualChosenCar();
        UpdateCarWheels();
        StartCoroutine(lastSecurePositionRefresh());
        //UpdateCarHat();
    }

    private void FixedUpdate() {
        
		if(Input.GetKey("up"))
			destroyPlayer("");
        // Manage of car properties and the changes of it from GlobalVariables
        ManageCarProperties();
        // Manage of nitro taken
        ManageNitro();
        // update engine motor pitch
        UpdatePitchEngine(GetVerticalAxis(), GetHorizontalAxis());
    }

    private void ManageCarProperties(){
        //Refresc de posició
        if(Time.time - timeSentinelRaycast >= 0.2f){

            if(!GlobalVariables.Instance.turnedCar && carController.turned){
                stuntsController.TriggerAnimation(Constants.ANIMATION_TRIGGER_TURN_UP);
            }
            // Deteccio canvi de cotxe
            if(actualCarEquipped != GlobalVariables.Instance.GetEquippedCarIndex()){
                UpdateActualChosenCar();
                GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_SMOKE_HIT, this.transform.position);
            }
            // Deteccio canvi de rodes
            else if(actualWheel.keyCode != GlobalVariables.Instance.GetEquippedWheelIndex()){
                UpdateCarWheels();
                GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_SMOKE_HIT, this.transform.position);
            }
            //TODO: hacer hats 
            //else if(hat.keyCode != GlobalVariables.Instance.GetEquippedHatIndex()){
            //    UpdateCarHat();
            //}

                
            switch(carController.currentStreetType){
                case StreetType.asphalt:
                break;

                case StreetType.water:
                    destroyPlayer("Water contact");
                break;

                case StreetType.grass:
                break;
            };
            timeSentinelRaycast = Time.time;
        }
    }
    public void UpdatePlayerCarInformation(CarInfo carInfo){
        // Basic info
        carController.idealRPM = carInfo.idealRPM;
        carController.maxRPM = carInfo.maxRPM;
        carController.turnRadius = carInfo.turnRadius;
        carController.torque = carInfo.torque;
        carController.brakeTorque = carInfo.brakeTorque;

        // Destructable parts meshes
        if(parts.Count != carInfo.parts.Count)
            ResizeDestructablePartsList(parts, carInfo.parts);
        for(int i =0; i < parts.Count; i++){
            MeshFilter mf = parts[i].GetComponent<MeshFilter>();
            MeshFilter newMf = carInfo.parts[i].GetComponent<MeshFilter>();
            mf.sharedMesh = newMf.sharedMesh;
            
            var bcol = parts[i].GetComponent<BoxCollider>();
            var bcolNew = carInfo.parts[i].GetComponent<BoxCollider>();
            bcol.center = bcolNew.center;
            bcol.size = bcolNew.size;
        }

        // Wheel new positions
        carController.wheelFL.transform.localPosition = carInfo.wheels[0].localPosition;
        carController.wheelFR.transform.localPosition = carInfo.wheels[1].localPosition;
        carController.wheelRL.transform.localPosition = carInfo.wheels[2].localPosition;
        carController.wheelRR.transform.localPosition = carInfo.wheels[3].localPosition;

        // Main mesh
        MeshFilter mainNewMf = carInfo.gameObject.GetComponent<MeshFilter>();
        mainMeshFilter.sharedMesh = mainNewMf.sharedMesh;

        transform.localScale = carInfo.gameObject.transform.localScale;
    }

    private void ResizeDestructablePartsList(List<PlayerDestructablePart> localParts, List<PlayerDestructablePart> newParts){
        if(localParts.Count > newParts.Count){
            // reduce parts to get the new content
            while(localParts.Count > newParts.Count){
                var partToDelete = localParts[localParts.Count-1];
                localParts.Remove(partToDelete);
                Destroy(partToDelete.gameObject);
            }
        }
        else{
            // augment parts to get new content.
            for(int i =0; i < newParts.Count; i++){
                if(i >= localParts.Count){
                   var newPart = Instantiate(newParts[i].gameObject, localParts[localParts.Count-1].transform.parent);
                   localParts.Add(newPart.GetComponent<PlayerDestructablePart>());
                }
            }
        }
    }

    
    private void destroyPlayer(string reason)
    {
        if(carController.turned){
            carController.turned = false;
            GlobalVariables.Instance.turnedCar = carController.turned;
            turnedUpParticle.gameObject.SetActive(carController.turned);
        }
        
            stuntsController.TriggerAnimation(Constants.ANIMATION_NAME_EXPLODE_BOOL);
            stuntsController.GetGUIController().startGameOver(reason);
        
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
        transform.position = v;
    }

    
    public void UpdateActualChosenCar(){
        GameObject go = GlobalVariables.Instance.LoadActualPlayerCar();
        CarInfo newPlayerObject = go.GetComponent<CarInfo>();
        if(newPlayerObject != null){
            UpdatePlayerCarInformation(newPlayerObject);
            stuntsController.UpdatePlayerAnimationSpeed(newPlayerObject.stuntHability);
            actualCarEquipped = GlobalVariables.Instance.GetEquippedCarIndex();
            PlayRunningCarChunk();
            var basecolNew = go.GetComponent<BoxCollider>();
            collider.center = basecolNew.center;
            collider.size = basecolNew.size;
            Destroy(go.gameObject);
        }
    }

    internal void UpdateCarWheels(){
        ShopWheel newWheel = GlobalVariables.Instance.LoadActualPlayerWheel();
        if(newWheel != null)
            actualWheel = newWheel;
        //Destroy(wheelGO.gameObject);
    }

    internal void UpdateCarHat(){
        ShopHat newHat = GlobalVariables.Instance.LoadActualPlayerHatPreset();
        if(newHat != null)
            actualHat = newHat;
        //Destroy(hatGO.gameObject);
    }

    internal void RecoverParts()
    {
        foreach (PlayerDestructablePart dp in parts)
        {
            if (dp.destroyed) dp.Recover();
        }
    }

    internal float GetVerticalAxis(){
        return carController.GetVerticalAxis();
    }


    internal float GetHorizontalAxis(){
        return carController.GetHorizontalAxis();
    }

    internal void PlayInitCarChunk(){
        if(RunningCarAudioSource != null)
            RunningCarAudioSource.PlayOneShot(initCarChunk);
    }

    internal void PlayRunningCarChunk(){
        if(RunningCarAudioSource != null){
            RunningCarAudioSource.clip = runningCarChunk;
            RunningCarAudioSource.Play();
        }
    }

    internal IEnumerator lastSecurePositionRefresh(){
        while(true)
         {
            yield return new WaitForSeconds(5);
            if(carController.currentStreetType != StreetType.water)
               lastSecurePositionPlayer = transform.position;
            
         }
    }

    internal int GetDestructablePartsCount(){
        return parts.Count;
    }

    private void ManageNitro()
    {
        // nitro flag detect control
        if (GlobalVariables.Instance.nitroflag)
        {
            GlobalVariables.Instance.nitroflag = false;
            GlobalVariables.Instance.GetAndPlayChunk(Constants.CHUNK_HIT_NITRO, 1.0f);
            stuntsController.UpdatePlayerAnimationBool(Constants.ANIMATION_NAME_NITRO_BOOL, true);
            StartCoroutine(carController.initializeNitro());
        }
        // Radial blur shader control
        if(stuntsController.GetPlayerAnimationBoolState(Constants.ANIMATION_NAME_NITRO_BOOL))
        	GlobalVariables.Instance.currentRadialBlur += 0.01f;
		else if(GlobalVariables.Instance.currentRadialBlur > 0f)
			GlobalVariables.Instance.currentRadialBlur -= 0.02f;

        // stabilize car if not touches ground
		if(!carController.grounded)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f), Time.deltaTime * 10);
    }
    public void startGameOver()
    {
        carController.canMove = false;
    }

    public void startGame()
    {
        carController.canMove = true;
    }

    public void UpdateCanMove(bool state){
        carController.canMove = state;
    }

    internal void UpdatePitchEngine(float vertical, float horizontal){
        if(RunningCarAudioSource != null){
            RunningCarAudioSource.pitch = carController.canMove ? Mathf.Lerp(RunningCarAudioSource.pitch, 0.25f + Math.Abs(vertical) - (Math.Abs(horizontal) / 2f), 10 * Time.deltaTime) : 0.5f;
            RunningCarAudioSource.volume = GlobalVariables.Instance.GetChunkLevel();
        }
        if(horizontal != 0 && carController.Rpm() > 700 && carController.grounded){
            if(!SkidCarAudioSource.isPlaying) SkidCarAudioSource.Play();
            //SkidCarAudioSource.pitch = Math.Abs(horizontal);
            SkidCarAudioSource.volume = GlobalVariables.Instance.GetChunkLevel();
        }
        else SkidCarAudioSource.Pause();

    }

    internal void StartGameWon(){
        stuntsController.GetGUIController().StartGameWon();
    }
    internal bool InitStunt(Stunt stunt){
        return stuntsController.InitStunt(stunt);
    }
    public void OnCollisionEnter(Collision collision)
    {
        communicatePlayerBaseCollition(collision);
    }

    internal void communicatePlayerBaseCollition(Collision collision)
    {
        if (collision.gameObject.tag.Contains(Constants.GO_TAG_CONTAINS_OBSTACULO) && !carController.turned)
            ComunicateCollisionPart(null, collision.collider);
    }

    internal void ComunicateCollisionPart(PlayerDestructablePart partDestroyed, Collider collision)
    {
        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if(obstacle != null){
            
            if (obstacle.penalizableObstacle)
            {
                // stop car's rigidbody
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GlobalVariables.Instance.GetAndPlayChunk(Constants.CHUNK_HIT_PLAYER, 1.0f);
                // conseqüencies de col·licio
                stuntsController.ResetComboStunt();
                if (obstacle.lethal) destroyPlayer(Constants.GAME_OVER_LETHAL_OBS_COLLIDED);
                else
                {
                    if (partDestroyed == null)
                        partDestroyed = findPartNotDestroyed();
                    if (partDestroyed != null){
                        int indexPartToDestroy = parts.IndexOf(partDestroyed);
                        if (!stuntsController.GetPlayerAnimationBoolState(Constants.ANIMATION_NAME_HIT_BOOL))
                        {
                            //stuntsController.SetAnimationOverriderControllerAnimation("playerHitAndRecovery", playerAnimator.runtimeAnimatorController.animationClips[0]);
                            if(stuntsController.trickMode) stuntsController.communicateStuntReset();
                            // executing particles from GlobalVariables
                            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_BOOM, partDestroyed.transform.position);
                            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_HIT, partDestroyed.transform.position);
                            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_SMOKE_HIT, partDestroyed.transform.position);
                            // shader effects
                            GlobalVariables.Instance.currentBrokenScreen = 0.05f * (indexPartToDestroy + 1);
                            GlobalVariables.Instance.shakeParam += 2.5f;
                            // si esta en mode stunt cancelar·lo perque el cotxe ha xocat
                            if(stuntsController.trickMode) {
                                stuntsController.UpdatePlayerAnimationBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, false);
                                stuntsController.trickMode = false;
                            }
                            
                            // habilitant boolea de col·licio de cotxe
                            stuntsController.UpdatePlayerAnimationBool(Constants.ANIMATION_NAME_HIT_BOOL, true);
                            // ejectant part del cotxe per mostrar danys per colisió
                            if(GlobalVariables.Instance.IsLevelGameState()){
                                stuntsController.DecrementGUIPart();
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

    private PlayerDestructablePart findPartNotDestroyed()
    {
        foreach (PlayerDestructablePart part in parts)
        {
            if (!part.destroyed) return part;
        }
        return null;
    }
    
    public void GenerateMoveParticleEffect(){
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
    }

}
