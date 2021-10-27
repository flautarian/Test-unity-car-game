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
    public float lightLevel;
    public List<PoolLoader> availablePrefabs;
    public ObjectiveGameType objective;
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

}
