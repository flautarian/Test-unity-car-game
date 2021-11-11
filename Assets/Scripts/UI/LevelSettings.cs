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
    public int lightLevel;
    public List<PoolLoader> availablePrefabs;
    public ObjectiveGameType objective;
    public int objectiveTarget;
    public int spawnerMovableLevelLeft;
    public int spawnerMovableLevelRight;
    public int spawnerStaticLevelLeft;
    public int spawnerStaticLevelRight;
    public bool hasTimeLimit;
    public float timeLimit;
    public float secondsFallTouchedTerrain;
    public GrassType grassType;
    public SpecialEvents specialEvents;
    public bool fog;


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
        specialEvents = lvl.specialEvents;
        fog = lvl.fog;
    }

}
