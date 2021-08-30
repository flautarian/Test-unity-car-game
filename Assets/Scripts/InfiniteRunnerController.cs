using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRunnerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Calle lastCalle;
    void Start()
    {
        GameObject firstStreet = PoolManager.Instance.SpawnFromPool("oneToOneWayRoads", Vector3.zero, Quaternion.Euler(0,0,0));
        Calle firstStreetCalle = firstStreet.GetComponent<Calle>();
        lastCalle = firstStreetCalle;
        if (firstStreetCalle != null)
            firstStreetCalle.generateNextStreet(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
