using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum GameMode
{
    INFINITERUNNER, CHALLENGE, TESTING, WOLRDMAINMENU, MAINMENU, MULTIPLAYER
}

public enum InGamePanels{
    GAMEON, PAUSED, LEVELSELECTION
}

public enum PanelInteractionType{
        TAX_TYPE,
        MULTIPLAYER_TYPE,
        LIBRARY_TYPE,
        BRIDGE_TYPE,
        NO_INTERACTION
    }

public enum ObjectiveGameType{
    BOSS,
    RINGS,
    NUMBER_STUNTS,
    MIN_GAS,
    ASPHALT_ONLY_NUMBER_STREETS,
    NUMBER_STREETS,
    CIRCUIT
}

public enum GrassType{
    NORMAL,
    LAVA,
    ICE,
    STICKY,
    NO_GRASS
}

public enum SpecialEvents{
    BOMBER,
    METEORITES,
    LEFT_WIND,
    RIGHT_WIND,
    FLOODINGS,
    NONE
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

    // flag indicatiu de que s'ha completat el nivell correctament (passda linea de meta)
    public bool levelWon = false;
    // Flag per generar linea de meta per el cas d'haver complert els objectius del nivell
    public bool generateGoalLine = false;

    public int objectiveActualTarget = 0;

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
        var strgo = GameObject.FindGameObjectWithTag(Constants.GO_TAG_STREET_CONTAINER);
        if (strgo != null) streetsContainer = strgo.transform;

        var partgo = GameObject.FindGameObjectWithTag(Constants.GO_TAG_PARTICLE_CONTAINER);
        if (partgo != null) particlesContainer = partgo.transform;

        objectiveActualTarget = 0;

        saveGameData = GetComponent<SaveGame>();
        if(actualLevelSettings == null) actualLevelSettings = GetComponent<LevelSettings>();

        if(actualLevelSettings != null && 
        gameMode != GameMode.WOLRDMAINMENU && 
        gameMode != GameMode.MAINMENU) PoolManager.Instance.PreparePoolDataFromLevel(actualLevelSettings.availablePrefabs);

        if (gameMode == GameMode.INFINITERUNNER)
        {
            GameObject firstStreet = PoolManager.Instance.SpawnFromPool(Constants.POOL_ONE_TO_ONE_STREET, Vector3.zero, Quaternion.Euler(0, 0, 0), streetsContainer);
            Calle firstStreetCalle = firstStreet.GetComponent<Calle>();
            firstStreetCalle.secondsUntilDrown = 8f;
            lastCalle = firstStreetCalle;
            if (firstStreetCalle != null)
                firstStreetCalle.generateNextStreet(5);
        }
        else if(gameMode == GameMode.WOLRDMAINMENU){
            
        }
        //Debug.Log("I'm Awakening!!");
    }

    void Start()
    {
    }

    public void InvoqueCanvasPanelButton(PanelInteractionType pit){
        actualPanelInteractionType = pit;
    }

    public void DisableCanvasPanelButton(){
        actualPanelInteractionType = PanelInteractionType.NO_INTERACTION;
    }

    internal void ResetLevel(){
        UpdateMinZLimit(0);
        totalCoins =0;
        totalStuntEC =0;
        objectiveActualTarget =0;
        Scene m_Scene = SceneManager.GetActiveScene();
   		SceneManager.LoadScene(m_Scene.name);
    }

    internal void addCoins(int number)
    {
        totalCoins += number;
    }

    internal void addStuntEC(int number)
    {
        totalStuntEC += number;
        if(totalStuntEC > 100) totalStuntEC = 100;
    }

    internal void UpdateMinZLimit(float zAxis){
        if(gameMode != GameMode.INFINITERUNNER)return;
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

    public float GetSoundLevel(){
        return saveGameData.data.soundValue;
    }
    public void UpdateSoundLevel(float level){
        saveGameData.data.soundValue = level;
    }

    public float GetChunkLevel(){
        return saveGameData.data.chunkValue;
    }

    public void UpdateChunkLevel(float level){
        saveGameData.data.chunkValue = level;
    }

    public void SaveGame(){
        saveGameData.UpdateSaveGame();
    }

    public void PrepareGlobalToLevel(LevelSettings newLvl, GameMode gMode){
        actualLevelSettings.CopyFromLevel(newLvl);
        gameMode = gMode;
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
        if(!levelWon)
            streetAnimator.SetBool(Constants.ANIMATION_STREET_FALL_BOOL, true);
        UpdateMinZLimit(streetAnimator.transform.position.z);
        if(actualLevelSettings.objective == ObjectiveGameType.NUMBER_STREETS){
            objectiveActualTarget++;
            if(actualLevelSettings.objectiveTarget <= objectiveActualTarget)GenerateGoalLineObject();
        }
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

    public void IncreaseTravelledStreets(){
        
    }
}
