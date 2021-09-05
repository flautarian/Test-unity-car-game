using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoolManager : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject[] prefabs;
        public int size;
    }

    #region Singleton

    public static PoolManager Instance;
    private Transform poolContainer;

    private void Awake()
    {
        var goPc = GameObject.FindGameObjectWithTag("PoolContainer");
        if(goPc != null) poolContainer = goPc.transform;
        poolDictionary = new Dictionary<string, List<GameObject>>();
        foreach (Pool pool in pools)
        {
            List<GameObject> queueOfPool = new List<GameObject>();
            int prefabLoop = 0;
            for (int i = 0; i < pool.size; i++)
            {
                if (prefabLoop < pool.prefabs.Length - 1) prefabLoop++;
                else prefabLoop = 0;
                GameObject prefab = Instantiate(pool.prefabs[prefabLoop]);
                prefab.transform.parent = poolContainer;
                prefab.SetActive(false);
                queueOfPool.Add(prefab);
            }
            poolDictionary.Add(pool.tag, queueOfPool);
        }
        Instance = this;
    }

    #endregion

    #region pool manager

    public List<Pool> pools;

    public Dictionary<string, List<GameObject>> poolDictionary;

    public GameObject SpawnFromPool( string tag, Vector3 position, Quaternion rotation, Transform origin)
    {
        if(poolDictionary != null) { 
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " is not found on pool list");
                return null;
            }

            List<GameObject> inactiveObjects = poolDictionary[tag].FindAll(go => !go.activeInHierarchy);
            // Check if the list created above has elements
            // If so, pick a random one,
            // Return null otherwise
            GameObject dequeueObject = inactiveObjects.Count > 0 ?
                inactiveObjects[Random.Range(0, inactiveObjects.Count)] :
                null;
            if(dequeueObject != null)
            {
                dequeueObject.SetActive(true);
                dequeueObject.transform.position = position;
                dequeueObject.transform.rotation = rotation;
                if(origin != null)dequeueObject.transform.parent = origin;
            }
            return dequeueObject;
        }
        return null;
    }

    #endregion
}
