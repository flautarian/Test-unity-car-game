using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Honeti;

public enum GameMode
{
    INFINITERUNNER, CHALLENGE, TESTING, WOLRDMAINMENU, MAINMENU, MULTIPLAYER
}

public enum InGamePanels{
    GAMEON, PAUSED, LEVELSELECTION, GAMELOST, GAMEWON, SUBUI1
}

public enum PanelInteractionType{
        TAX_TYPE,
        MULTIPLAYER_TYPE,
        LIBRARY_TYPE,
        BRIDGE_TYPE,
        INFO_PANEL_TYPE,
        CONCESSIONARY_PANEL_TYPE,
        NO_INTERACTION,
        CHALLENGE_TYPE,
        RELICS_TYPE
    }

public enum ObjectiveGameType{
    BOSS,
    ORBS,
    NUMBER_AERIAL_STUNTS,
    NUMBER_GROUNDAL_STUNTS,
    NUMBER_ESPECIFIC_STUNTS,
    NUMBER_STREETS,
    NUMBER_COINS,
    CIRCUIT
}



public enum GrassType{
    GREEN,
    DESSERT,
    ICE,
    GROUND,
    NORMAL
}

public enum Mutator{
    NONE,
    BOMBER,
    METEORITES,
    LEFT_WIND,
    RIGHT_WIND,
    FLOODINGS,
    LAVA,
    WET_ROAD,
    WET_GRASS,
    STICKY_ROAD,
    STICKY_GRASS,
    NO_GRASS
}


public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance { get; private set; }

    // actualitzacio de nivell de il·luminacio global
    public float currentLight = 0;

    // comptador de punts de monedes
    public int totalCoins = 0;

    // comptador de punts de stunt
    public int totalStuntEC = 0;

    // actualitzacio d'aplicacio de nitro
    public bool nitroflag = false;

    // actualitzacio d'execucio de tasques de reparacio
    public bool repairflag = false;

    // actualitzacio del estat del sistema de stunts
    public StuntState castingStunt = StuntState.OFF;

    // parametre del shader de simulacio de pantalla trencada
    public float currentBrokenScreen = 0;

    // parametre de intensitat del shader radial blur
    public float currentRadialBlur = 0;

    // control de curvatura de shader bend
    public Vector3 curveControl = Vector3.zero;

    // parametre de moviment de la camara
    public float shakeParam = 0;

    // actualitzacio de la velocitat actualLevelSettings del jugador
    public float playerCurrentVelocity;

    // contenidor per a les perticles creades
    public static Transform particlesContainer;

    // contenidor per els carrers creats
    private Transform streetsContainer;

    // data warehouse per administrar particules
    private static Hashtable particleSystems = new Hashtable();

    // tipus de joc de l'escena
    public GameMode gameMode;

    // referencia a ultim carrer creat
    public Calle lastCalle;

    // eix minim z que es pot accedir del mapa
    public float minZLimit = -90000;

    // configuracio desada al sistema realcionada amb el joc
    private SaveGame saveGameData;

    // estat del joc en questio d'opcions
    public InGamePanels inGameState = InGamePanels.GAMEON;

    // Actual proximitat amb entorns i menus interactuables
    public PanelInteractionType actualPanelInteractionType;

    // Configuracio del nivell escollit
    public LevelSettings actualLevelSettings;

    // Flag per generar linea de meta per el cas d'haver complert els objectius del nivell
    public bool generateGoalLine = false;
    // Flag per generar activar el sistema de cotxe caigut per recuperar-lo prement <- ->
    public bool turnedCar = false;

    public int objectiveActualTarget = 0;

    //Control de l'objectiu de la camara principal
    public Cinemachine.CinemachineVirtualCamera mainCameraControl;

    // Transforms auxiliars de seguiment de camera
    private Transform cameraMainLookTransform, cameraMainFollowTransform;

    public Transform cameraLookFocusTransform, cameraFollowFocusTransform;
    //Gestor d'events de UI del joc
    public EventSystem eventSystem;

    // Connexio amb audioSource dels chunks generals
    [SerializeField]
    private AudioSource chunkSource;

    [SerializeField]
    internal AudioSource musicSource;

    internal List<string> pendingNotifications = new List<string>();

    public bool playerTargetedByCamera = true;

    public int stuntCombo = 0;

    internal Vector3 lastVisitedBuildingPositionPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        var es = GameObject.Find("EventSystem");
        eventSystem = es.GetComponent<EventSystem>();
        
        var strgo = GameObject.FindGameObjectWithTag(Constants.GO_TAG_STREET_CONTAINER);
        if (strgo != null) streetsContainer = strgo.transform;

        var partgo = GameObject.FindGameObjectWithTag(Constants.GO_TAG_PARTICLE_CONTAINER);
        if (partgo != null) particlesContainer = partgo.transform;

        objectiveActualTarget = 0;

        saveGameData = GetComponent<SaveGame>();
        if(actualLevelSettings == null) actualLevelSettings = GetComponent<LevelSettings>();

        if(actualLevelSettings != null && IsLevelGameState()) 
            PoolManager.Instance.PreparePoolDataFromLevel(actualLevelSettings.availablePrefabs);
        
        if (gameMode == GameMode.INFINITERUNNER)
        {
            GameObject firstStreet = PoolManager.Instance.SpawnFromPool(Constants.POOL_ONE_TO_ONE_STREET, Vector3.zero, Quaternion.Euler(0, 0, 0), streetsContainer);
            Calle firstStreetCalle = firstStreet.GetComponent<Calle>();
            firstStreetCalle.secondsUntilDrown = 8f;
            lastCalle = firstStreetCalle;
            if (firstStreetCalle != null)
                firstStreetCalle.generateNextStreet(5);
        }
        //Debug.Log("GlobalVariables Awakening!!");
    }

    public void InvoqueCanvasPanelButton(PanelInteractionType pit, Transform focus, Transform follow){
        actualPanelInteractionType = pit;
        updateMainCameraLookAt(focus, follow);
    }

    public void DisableCanvasPanelButton(){
        actualPanelInteractionType = PanelInteractionType.NO_INTERACTION;
        updateMainCameraLookAt(null, null);
        switchCameraFocusToSecondaryObject(false, false);
    }

    internal void ResetLevel(){
        ResetAllGameOnFlags();
        Scene m_Scene = SceneManager.GetActiveScene();
        FadeOutActualSong();
   		SceneManager.LoadScene(m_Scene.name);
    }

    internal void GoToTaxLevel(){
        ResetAllGameOnFlags();
        FadeOutActualSong();
        Debug.Log(actualLevelSettings.sceneName);
        if(actualLevelSettings.sceneName != "")
            SceneManager.LoadScene(actualLevelSettings.sceneName);
        else
            SceneManager.LoadScene(2);
    }

    public void CleanBeforeChangeScene(){
        FadeOutActualSong();
        ResetAllGameOnFlags();
    }

    private void ResetAllGameOnFlags(){
        UpdateMinZLimit(0);
        ResetShaders();
        totalStuntEC =0;
        totalCoins =0;
        objectiveActualTarget =0;
        nitroflag = false;
        repairflag = false;
        inGameState = InGamePanels.GAMEON;
    }

    public void ResetShaders(){
        currentRadialBlur = 0;
        currentBrokenScreen = 0;
    }

    internal GameObject LoadActualPlayerCar(){
        UnityEngine.Object newCar = (UnityEngine.Object)Resources.Load("Prefabs/Cars/" + GetActualPlayerCarPrefabName());
        GameObject carGO = (GameObject) Instantiate(newCar);
        return carGO;
    }

    private string GetActualPlayerCarPrefabName(){
        return Constants.PREFAB_CAR_NAMES[saveGameData.data.equippedCar];
    }

    
    public bool IsScrollEnabled(int stuntIndex){
        return saveGameData.data.scrolls[stuntIndex].unlocked;
    }

    public bool IsRelicEnabled(int index){
        return saveGameData.data.relics[index] == 1;
    }

    internal Scroll[] GetSavedScrolls(){
        return saveGameData.data.scrolls;
    }

    internal Scroll GetScroll(int key){
        return key >= 0 ? saveGameData.data.scrolls[key] : null;
    }

    internal void UnlockScroll(int key){
        saveGameData.data.scrolls[key].unlocked = true;
        NotifyScrollUnlocked(key);
        //saveGameData.UpdateSaveGame();
    }

    internal void NotifyActualLvlObjective(){
        pendingNotifications.Add( "^detail_lvl_panel_objective_" + (int)actualLevelSettings.objective + "|" + actualLevelSettings.objectiveTarget + "|" + actualLevelSettings.objectiveEspecification);
    }

    internal void NotifyToUISystem(string msg){
        if(pendingNotifications.IndexOf(msg) < 0)
            pendingNotifications.Add(msg);
    }

    internal void UnlockRelic(int key){
        saveGameData.data.relics[key] = 1;
        NotifyRelicUnlocked(key);
        //saveGameData.UpdateSaveGame();
    }

    internal void UpdateCoinsOfPlayerForLvlSuccessfull(int coins){
        totalCoins += coins;
        NotifyCoinsGained(coins);
    }

    private void NotifyCoinsGained(int coins){
        pendingNotifications.Add("^coins_unlocked_notification|" + coins);
    }

    private void NotifyRelicUnlocked(int key){
        pendingNotifications.Add("^relic_unlocked_notification|" + I18N.instance.getValue("^relic_name_" + key));
    }

    private void NotifyScrollUnlocked(int key){
        pendingNotifications.Add("^stunt_unlocked_notification|" + Constants.STUNT_NAMES[key]);
    }

    internal void UpdateEquipedScroll(int index, int scrollKey){
        saveGameData.data.equippedScrolls[index] = scrollKey;
    }

    internal Stunt[] GenerateStuntListWithEquippedStunts(){
        Stunt[] result = new Stunt[4];
        int[] stuntsToLoad = GetStuntsToLoad();
        for(int s = 0; s < stuntsToLoad.Length; s++){
            if(saveGameData.data.equippedScrolls[s] >= 0){
                UnityEngine.Object newScroll = (UnityEngine.Object)Resources.Load("Prefabs/Stunts/" + Constants.STUNT_NAMES[saveGameData.data.equippedScrolls[s]]);
                GameObject scrollGO = (GameObject)Instantiate(newScroll);
                Stunt st = scrollGO.GetComponent<Stunt>();
                result[s] = st;
            }
            else result[s] = null;
        }
        return result;
    }

    private int[] GetStuntsToLoad(){
        if(IsLevelGameState() && actualLevelSettings.StuntOverride.Length > 0)
            return actualLevelSettings.StuntOverride;
        return saveGameData.data.equippedScrolls;
    }
    
    internal Scroll[] GetPlayerEquippedScrolls(){
        Scroll[] result = new Scroll[4];
        for(int i = 0; i < saveGameData.data.equippedScrolls.Length; i++){
            result[i] = GetScroll(saveGameData.data.equippedScrolls[i]);
        }
        return result;
    }

    internal void addCoins(int number)
    {
        GetAndPlayChunk(Constants.CHUNK_HIT_COIN, ((float) UnityEngine.Random.Range(9f, 13f) / 10f));
        totalCoins += (number * (stuntCombo <= 0 ? 1 : stuntCombo));
        AddObjectivePoint(ObjectiveGameType.NUMBER_COINS, number);
    }

    internal void addStuntEC(int number)
    {
        totalStuntEC += number;
        if(totalStuntEC > 100) totalStuntEC = 100;
        AddObjectivePoint(ObjectiveGameType.ORBS, number);
    }

    internal void substractStuntEC(int number)
    {
        totalStuntEC -= number;
        if(totalStuntEC < 0) totalStuntEC = 0;
    }

    internal void AddObjectivePoint(int number){
        objectiveActualTarget += number;
        if(objectiveActualTarget >= actualLevelSettings.objectiveTarget &&
                !generateGoalLine) {
            GenerateGoalLineObject();
            GetAndPlayChunk("Objective_Unlocked", 1.0f);
        }
        else if(!generateGoalLine){
            GetAndPlayChunk("Objective_Add", 0.75f);
        }
    }

    internal void AddObjectivePoint(ObjectiveGameType objective, int number){
        if(objective == actualLevelSettings.objective){
            AddObjectivePoint(number);
        }
    }

    internal void UpdateMinZLimit(float zAxis){
        if(gameMode != GameMode.INFINITERUNNER) return;
        minZLimit = zAxis;
    }
    public static UnityEngine.Object RequestParticleSystem(String identifier)
    {
        if (!particleSystems.Contains(identifier))
        {
            particleSystems.Add(identifier, Resources.Load("Particles/" + identifier));
        }
        return (UnityEngine.Object)particleSystems[identifier];
    }

    internal static void RequestAndExecuteParticleSystem(string particlePrefabName, Vector3 position)
    {
        UnityEngine.Object PartObject = GlobalVariables.RequestParticleSystem(particlePrefabName);
        if (PartObject != null)
        {
            GameObject newParticle = (GameObject)Instantiate(PartObject);
            if (particlesContainer != null) newParticle.transform.parent = particlesContainer.transform;
            newParticle.transform.position = position;
            newParticle.gameObject.SetActive(true);
        }
    }

    public float GetHSensibilityLevel(){
        return saveGameData.data.hSensibility;
    }
    public int GetFOVLevel(){
        return saveGameData.data.farClipPlane;
    }

    public int GetFarCameraLevel(){
        return saveGameData.data.farCamera;
    }

    public float GetSoundLevel(){
        return saveGameData.data.soundValue;
    }

    public string GetLanguage(){
        return saveGameData.data.language;
    }
    public void UpdateSoundLevel(float level){
        saveGameData.data.soundValue = level;
    }

    public void UpdateHSensibilityLevel(float level){
        saveGameData.data.hSensibility = level;
    }

    public void UpdateFOVLevel(float level){
        saveGameData.data.farClipPlane =  75 + (int) (400 * level);
        if(mainCameraControl == null) UpdateMainCameraAttribute();
        if(mainCameraControl != null) mainCameraControl.m_Lens.FarClipPlane = saveGameData.data.farClipPlane;
    }

    public void UpdateFarCameraLevel(float level){
        saveGameData.data.farCamera =  75 + (int) (50 * level);
        if(mainCameraControl == null) UpdateMainCameraAttribute();
        if(mainCameraControl != null) mainCameraControl.m_Lens.FieldOfView = saveGameData.data.farCamera;
    }

    public float GetChunkLevel(){
        return saveGameData.data.chunkValue;
    }

    public void UpdateChunkLevel(float level){
        saveGameData.data.chunkValue = level;
    }

    public void SaveGame(){
        saveGameData.RefreshKeyCodeBindings();
        saveGameData.UpdateSaveGame();
    }

    public void PrepareGlobalToLevel(LevelSettings newLvl){
        actualLevelSettings.CopyFromLevel(newLvl);
    }

    public int getSpawnerMovableMaxTime(SpawnerOrientation orientation){
        return orientation == SpawnerOrientation.LEFT ? actualLevelSettings.spawnerMovableLevelLeft : actualLevelSettings.spawnerMovableLevelRight;
    }

    public int getSpawnerStaticMaxTime(SpawnerOrientation orientation){
        return orientation == SpawnerOrientation.LEFT ? actualLevelSettings.spawnerStaticLevelLeft : actualLevelSettings.spawnerStaticLevelRight;
    }

    public List<LevelSettings.PoolLoader> getLoadedPools(){
        return actualLevelSettings.availablePrefabs;
    }

    public void ManageStreetGeneration(Animator streetAnimator){
        if(inGameState == InGamePanels.GAMEON)
            streetAnimator.SetBool(Constants.ANIMATION_STREET_FALL_BOOL, true);
        UpdateMinZLimit(streetAnimator.transform.position.z);
        AddObjectivePoint(ObjectiveGameType.NUMBER_STREETS, 1);
    }

    public int GetActualObjectiveTarget(){
        return actualLevelSettings.objectiveTarget;
    }

    public void GenerateGoalLineObject(){
        if(!generateGoalLine){
            UnityEngine.Object obj = (UnityEngine.Object)Resources.Load("Prefabs/Goal");
            GameObject goalGameObject = (GameObject)Instantiate(obj);
            var pos = lastCalle.transform.position;
            pos.x = 10;
            goalGameObject.transform.position = pos;
            generateGoalLine = true;
        }
    }

    public void prepareSceneWithSaveGameParametters(){
        if(mainCameraControl == null) UpdateMainCameraAttribute();
        if(mainCameraControl != null){
            mainCameraControl.m_Lens.FarClipPlane = saveGameData.data.farClipPlane;
            mainCameraControl.m_Lens.FieldOfView = saveGameData.data.farCamera;
        }
        if(I18N.instance != null)
            I18N.instance.setLanguage(saveGameData.data.language);
        totalCoins = IsLevelGameState() ? 0 : saveGameData.data.totalCoins;
        UpdateVolumeAudioSources();
    }

    public void UpdateVolumeAudioSources(){
        chunkSource.volume = saveGameData.data.chunkValue;
        musicSource.volume = saveGameData.data.soundValue;
    }

    public void UpdateLevelState(InGamePanels newState){
        inGameState = newState;
    }

    public int GetLvlTime(){
        return actualLevelSettings.lightLevel;
    }

    public void updateMainCameraLookAt(Transform t, Transform f){
        if(mainCameraControl == null) UpdateMainCameraAttribute();
        if(t != null)
            cameraLookFocusTransform = t;
        else 
            cameraLookFocusTransform = cameraMainLookTransform;
        if(f != null)
            cameraFollowFocusTransform = f;
        else cameraFollowFocusTransform = cameraMainFollowTransform;

    }

    public void switchCameraFocusToSecondaryObject(bool focusSecondary, bool followSecondary){
        if(mainCameraControl == null) UpdateMainCameraAttribute();
        mainCameraControl.m_LookAt = focusSecondary ? cameraLookFocusTransform : cameraMainLookTransform;
        mainCameraControl.m_Follow = followSecondary ? cameraFollowFocusTransform : cameraMainFollowTransform;
        playerTargetedByCamera = !focusSecondary;
    }

    public void SetFocusUiElement(GameObject go){
        ClearFocusUiElement();
        eventSystem.SetSelectedGameObject( go, new BaseEventData(eventSystem));
        eventSystem.firstSelectedGameObject = go;
    }

    private void ClearFocusUiElement(){
        if(eventSystem == null){
            var es = GameObject.Find("EventSystem");
            eventSystem = es.GetComponent<EventSystem>();
        }
        eventSystem.firstSelectedGameObject = null;
        eventSystem.SetSelectedGameObject( null, new BaseEventData(eventSystem));
    }

    public void UpdateActiveLanguage(string value){
        saveGameData.data.language = value;
        I18N.instance.setLanguage(value);
    }

    public void UpdateKeyBinding(int key, KeyCode keyCode){ 
        saveGameData.data.keyBindings[key] = keyCode.ToString();
    }

    public string GetKeyBindingText(int key){ 
        return saveGameData.data.keyBindings[key];
    }

    public KeyCode GetKeyCodeBinded(int key){ 
        return saveGameData.GetKeyCodeBinded(key);
    }

    public string GetSavedKeyButton(int key){
       return saveGameData.data.keyBindings[key];
    }

    public bool IsMutatorActive(Mutator mutator){
        if(gameMode != GameMode.INFINITERUNNER && gameMode != GameMode.CHALLENGE)return false;
        foreach(Mutator m in actualLevelSettings.mutators){
            if(m == mutator) return true;
        }
       return false;
    }

    public void UpdateGamemodeFromLvlSettings(){
        if(actualLevelSettings != null)
            gameMode = actualLevelSettings.gameMode;
    }

    private void UpdateMainCameraAttribute(){
        var mainCameraCGO = GameObject.FindGameObjectWithTag("MainVirtualCamera");
        if(mainCameraCGO != null){
            mainCameraControl = mainCameraCGO.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            cameraMainLookTransform = mainCameraControl.m_LookAt;
            cameraMainFollowTransform = mainCameraControl.m_Follow;
            cameraLookFocusTransform = cameraMainLookTransform;
            playerTargetedByCamera = true;
        }
        else Debug.Log("MainVirtualCamera not found!");
    }

    public bool GetBuyStatusCar(int value){
        // 0 = not bought, 1 = bought
        return saveGameData.data.cars[value] == 1;
    }

    public void UnlockCar(int key, int price){
        saveGameData.data.cars[key] = 1;
        saveGameData.data.totalCoins -= price;
        SaveGame();
    }

    public void EquipCar(int key){
        // 0 = not bought, 1 = bought
        saveGameData.data.equippedCar = key;
        SaveGame();
    }

    public int GetEquippedCarIndex(){
        return saveGameData.data.equippedCar;
    }

    public bool IsCompletedLevel(int value){
        if(value < 0) return true;
        return saveGameData.data.levels[value].done;
    }

    public bool IsCompletableLevel(int value){
        Level lvl = saveGameData.data.levels[value];
        if(lvl.previousLevel < 0) return true;
        else return saveGameData.data.levels[lvl.previousLevel].done;
    }

    public void UpdateEquippedCar(int value){
        saveGameData.data.equippedCar = value;
    }

    public LevelSettings.PrizeLevel GetActualLevelPrizeType(){
        return actualLevelSettings.prize;
    }

    public string GetLevelObjectiveEsp(){
        return actualLevelSettings.objectiveEspecification;
    }

    public ObjectiveGameType GetLevelObjective(){
        return actualLevelSettings.objective;
    }

    public int GetLevelObjectiveTarget(){
        return actualLevelSettings.objectiveTarget;
    }

    public void SucceessActualLevel(){
        if(gameMode == GameMode.INFINITERUNNER)
            saveGameData.data.levels[actualLevelSettings.lvlIndex].done = true;
        else if (gameMode == GameMode.CHALLENGE)
            saveGameData.data.challenges[actualLevelSettings.lvlIndex] = 1.0f;
        if(GetActualLevelPrizeType() == LevelSettings.PrizeLevel.COINS)
            UpdateCoinsOfPlayerForLvlSuccessfull(actualLevelSettings.prizeDetail);
        else if(GetActualLevelPrizeType() == LevelSettings.PrizeLevel.SCROLL)
            UnlockScroll(actualLevelSettings.prizeDetail);
        saveGameData.data.totalCoins += totalCoins;
    }

    public bool IsLevelGameState(){
        return gameMode == GameMode.INFINITERUNNER || gameMode ==  GameMode.CHALLENGE || gameMode == GameMode.MULTIPLAYER;
    }

    public bool IsMainMenuGameState(){
        return gameMode == GameMode.MAINMENU;
    }
    public bool IsWorldMenuGameState(){
        return gameMode == GameMode.WOLRDMAINMENU;
    }

    public void GetAndPlayChunk(string chunk, float ptch){
        AudioClip clip = PoolManager.Instance.SpawnChunkFromPool(chunk);
        chunkSource.pitch = ptch;
        chunkSource.PlayOneShot(clip);
    }

    public void GetAndPlaySong(string song){
        if(musicSource.isPlaying) musicSource.Stop();
        AudioClip clip = PoolManager.Instance.SpawnSongFromPool(song);
        if(clip != null) musicSource.clip = clip;
        musicSource.Play();
    }

    internal IEnumerator PlayDefaultSceneSong(){
        if(musicSource != null){
            while (!musicSource.enabled) yield return null;
            Scene m_Scene = SceneManager.GetActiveScene();
            if(PoolManager.Instance != null){
                AudioClip clip = PoolManager.Instance.SpawnSongFromPool(Constants.DEFAULT_SCENE_SONGS[m_Scene.buildIndex]);
                if(clip != null){
                    musicSource.clip = clip;
                    musicSource.Play(); 
                }
                else Debug.Log("Error, clip not found!");
            }else Debug.Log("Error, pool not initialized!");
        }
    }

    public void UpdateLastVisitedBuildingPlayer(Vector3 v){
        lastVisitedBuildingPositionPlayer = v;
    }

    public void FadeOutActualSong(){
        musicSource.Stop();
    }


}
