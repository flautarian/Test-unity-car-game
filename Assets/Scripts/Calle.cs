using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Calle : MonoBehaviour
{
    // Start is called before the first frame update

    public WayPointManager waypointManager;
    private BoxCollider calleBounds;

    private void Awake()
    {
        calleBounds = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        // com es l'ultima que es lleva es l'ultim carrer
        GlobalVariables.Instance.lastCalle = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Calle lastCalle = GlobalVariables.Instance.lastCalle;
            if(lastCalle != null)
            {
                lastCalle.generateNextStreet(1);
                StartCoroutine(DestroyStreet());
            }
        }
    }

    public IEnumerator DestroyStreet()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    public void generateNextStreet(int streetsRemainingToGenerate)
    {
        GameObject newStreet = generateNextStreetGO();
        streetsRemainingToGenerate--;
        
        if(newStreet != null)
        {
            Calle newCalle = GlobalVariables.Instance.lastCalle;
            updatePaths(newCalle);
            if (streetsRemainingToGenerate > 0) newCalle.generateNextStreet(streetsRemainingToGenerate);
        }
    }

    private void updatePaths(Calle newStreet)
    {
        WayPointManager newWpm = newStreet.waypointManager;
        // interconnectem els carrils dels npcs entre l'ultima street i la nova que entra
        newWpm.addPreviousWayPoint(waypointManager.lastWayPoint);
        waypointManager.addToNextWayPoint(newWpm.firstWayPoint);
        waypointManager.addNextReversalWayPoint(newWpm.lastWayReversalPoint);
        newWpm.addPreviousReversalWayPoint(waypointManager.firstWayReversalPoint);
    }

    private GameObject generateNextStreetGO()
    {
        String nextStreetGroupTagName = getNextStreetTagName();
        return PoolManager.Instance.SpawnFromPool(nextStreetGroupTagName,
            new Vector3(transform.position.x, transform.position.y, transform.position.z + calleBounds.size.z - 0.5f),
            Quaternion.Euler(0,0,0));
    }

    private string getNextStreetTagName()
    {
        int leftSideStreetsNumber = waypointManager.lastWayReversalPoint.Count;
        int rightSideStreetsNumber = waypointManager.lastWayPoint.Count;
        if (leftSideStreetsNumber == 1 && rightSideStreetsNumber == 1) return "oneToOneWayRoads";
        else if (leftSideStreetsNumber == 2 && rightSideStreetsNumber == 1) return "twoToOneWayRoads";
        else if (leftSideStreetsNumber == 2 && rightSideStreetsNumber == 2) return "twoToTwoWayRoads";
        else if (leftSideStreetsNumber == 1 && rightSideStreetsNumber == 2) return "oneToTwoWayRoads";
        else return "oneToOneWayRoads";
    }

    /*internal void initializePowerUps(List<GameObject> powerUps)
    {
        GameObject[] pows = GameObject.FindGameObjectsWithTag("powerUpSpawnPoint");
        foreach(GameObject pow in pows)
        {
            GameObject newItem = Instantiate(powerUps[UnityEngine.Random.Range(0, powerUps.Count)]);
            newItem.transform.position = pow.transform.position;
            newItem.transform.parent = pow.transform.parent;
            Destroy(pow);
        }
    }*/
}
