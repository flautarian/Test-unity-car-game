using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [System.Serializable]
    public class PoolLoader
    {
        public string tag;
        public string[] prefabs;
        public int size;
        public string folder;
    }
    // estat del dia(0 = nit, 21600= mati, 43200 mitgdia, 64800 tarda)
    public int lightLevel;

    // carrers que utilitzara el nivell (En cas de infinite runner i runner organitzat)
    public List<PoolLoader> availablePrefabs;
    // objectiu per guanyar el nivell
    public ObjectiveGameType objective;
    // objectiu numeric per guanyar nivell
    public int objectiveTarget;
    // detall d'objectiu
    public string objectiveEspecification;
    // trafic en carrer esquerra
    public int spawnerMovableLevelLeft;
    // trafic en carrer dret
    public int spawnerMovableLevelRight;
    // trafic d'elements de la calçada dreta 
    public int spawnerStaticLevelLeft;
    // trafic d'elements de la calçada esquerra
    public int spawnerStaticLevelRight;
    // flag de limit de temps
    public bool hasTimeLimit;
    // quantitat de limit de temps
    public float timeLimit;
    // segons que espera cada tram abans de caure al mar
    public float secondsFallTouchedTerrain;
    // tipus de cesped
    public GrassType grassType;
    // mutadors
    public Mutator[] mutators;
    // flag de boira en el carrer
    public bool fog;
    // nom del nivell
    public string levelName;
    // tipus de nivell
    public GameMode gameMode;

    public void CopyFromLevel(LevelSettings lvl){
        lightLevel = lvl.lightLevel;
        availablePrefabs = lvl.availablePrefabs;
        objective = lvl.objective;
        objectiveTarget = lvl.objectiveTarget;
        spawnerMovableLevelLeft = lvl.spawnerMovableLevelLeft;
        spawnerMovableLevelRight = lvl.spawnerMovableLevelRight;
        spawnerStaticLevelLeft = lvl.spawnerStaticLevelLeft;
        spawnerStaticLevelRight = lvl.spawnerStaticLevelRight;
        hasTimeLimit = lvl.hasTimeLimit;
        timeLimit = lvl.timeLimit;
        secondsFallTouchedTerrain = lvl.secondsFallTouchedTerrain;
        grassType = lvl.grassType;
        mutators = lvl.mutators;
        fog = lvl.fog;
    }

}
