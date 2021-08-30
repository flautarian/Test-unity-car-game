using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    INFINITERUNNER, CHALLENGE
}

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance { get; private set; }

    public float currentLight = 0;

    public int totalCoins = 0;

    public float currentBrokenScreen = 0;

    public float currentRadialBlur = 0;

    public float shakeParam = 0;

    public static GameObject particlesPlace;

    private static Hashtable particleSystems = new Hashtable();

    public GameMode gameMode;

    public Calle lastCalle;

    public bool isGameInfinite;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        particlesPlace = GameObject.FindGameObjectWithTag("Particles");
    }

    void Start()
    {
        if(gameMode == GameMode.INFINITERUNNER) { 
            GameObject firstStreet = PoolManager.Instance.SpawnFromPool("oneToOneWayRoads", Vector3.zero, Quaternion.Euler(0, 0, 0));
            Calle firstStreetCalle = firstStreet.GetComponent<Calle>();
            lastCalle = firstStreetCalle;
            if (firstStreetCalle != null)
                firstStreetCalle.generateNextStreet(5);
        }
    }

    internal void addCoins(int number)
    {
        totalCoins += number;
    }

    public static UnityEngine.Object RequestParticleSystem(String identifier) {
        //return null;
      if ( !particleSystems.Contains(identifier ) ) {
            particleSystems.Add(identifier, Resources.Load( "Particles/" + identifier ) );
          }
      return (UnityEngine.Object)particleSystems[identifier];
    }

    internal static void RequestAndExecuteParticleSystem(string particlePrefabName, Vector3 position)
    {
        UnityEngine.Object PartObject = GlobalVariables.RequestParticleSystem(particlePrefabName);
        if(PartObject != null)
        {
            GameObject newParticle = (GameObject)Instantiate(PartObject);
            if (particlesPlace != null) newParticle.transform.parent = particlesPlace.transform;
            newParticle.transform.position = position;
            newParticle.gameObject.SetActive(true);
        }
    }
}
