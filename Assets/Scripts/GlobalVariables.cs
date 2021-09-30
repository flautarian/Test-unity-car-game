using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    INFINITERUNNER, CHALLENGE, WOLRDMAINMENU
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

    // actualitzacio de la velocitat actual del jugador
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
    }

    void Start()
    {
        if (gameMode == GameMode.INFINITERUNNER)
        {
            GameObject firstStreet = PoolManager.Instance.SpawnFromPool(Constants.POOL_ONE_TO_ONE_STREET, Vector3.zero, Quaternion.Euler(0, 0, 0), streetsContainer);
            Calle firstStreetCalle = firstStreet.GetComponent<Calle>();
            firstStreetCalle.secondsUntilDrown = 8f;
            lastCalle = firstStreetCalle;
            if (firstStreetCalle != null)
                firstStreetCalle.generateNextStreet(5);
        }
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
}
