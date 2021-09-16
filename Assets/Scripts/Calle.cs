using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Calle : MonoBehaviour
{
    // Start is called before the first frame update

    public WayPointManager waypointManager;
    public float secondsUntilDrown = 3f;
    private BoxCollider calleBounds;

    private Animator streetAnimationController;
    private void Awake()
    {
        streetAnimationController = GetComponent<Animator>();
        calleBounds = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        // com es l'ultima que es lleva es l'ultim carrer
        StartCoroutine(initializeCalle());
    }

    private IEnumerator initializeCalle()
    {
        while (GlobalVariables.Instance == null) yield return null;
        //Debug.Log("Ahora last calle es " + this.name);
        GlobalVariables.Instance.lastCalle = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Constants.GO_TAG_PLAYER))
        {
            if (GlobalVariables.Instance != null && GlobalVariables.Instance.gameMode.Equals(GameMode.INFINITERUNNER))
            {
                Calle lastCalle = GlobalVariables.Instance.lastCalle;
                if (lastCalle != null)
                {
                    lastCalle.generateNextStreet(1);
                    StartCoroutine(StartCountDownToDisableStreet());
                }
            }
        }
    }

    private IEnumerator StartCountDownToDisableStreet()
    {
        yield return new WaitForSeconds(secondsUntilDrown);
        streetAnimationController.SetBool("fall", true);
    }

    public void generateNextStreet(int streetsRemainingToGenerate)
    {
        GameObject newStreet = generateNextStreetGO();
        streetsRemainingToGenerate--;

        if (newStreet != null)
        {
            Calle newCalle = GlobalVariables.Instance.lastCalle;
            updatePaths(newCalle);
            if (streetsRemainingToGenerate > 0) newCalle.generateNextStreet(streetsRemainingToGenerate);
        }
    }


    private void updatePaths(Calle newStreet)
    {
        // interconnectem els carrils dels npcs entre l'ultima street i la nova que entra
        WayPointManager newWpm = newStreet.waypointManager;
        newWpm.addPreviousWayPoint(waypointManager.lastWayPoint);
        waypointManager.addToNextWayPoint(newWpm.firstWayPoint);
        waypointManager.addNextReversalWayPoint(newWpm.lastWayReversalPoint);
        newWpm.addPreviousReversalWayPoint(waypointManager.firstWayReversalPoint);
    }

    private GameObject generateNextStreetGO()
    {
        String nextStreetGroupTagName = getNextStreetTagName();
        return PoolManager.Instance.SpawnFromPool(nextStreetGroupTagName,
            new Vector3(transform.position.x, 0, transform.position.z + calleBounds.size.z - 0.5f),
            Quaternion.Euler(0, 0, 0), transform.parent);
    }

    private string getNextStreetTagName()
    {
        int leftSideStreetsNumber = waypointManager.firstWayReversalPoint.Count;
        int rightSideStreetsNumber = waypointManager.lastWayPoint.Count;
        if (leftSideStreetsNumber == 1 && rightSideStreetsNumber == 1) return Constants.POOL_ONE_TO_ONE_STREET;
        else if (leftSideStreetsNumber == 2 && rightSideStreetsNumber == 1) return Constants.POOL_TWO_TO_ONE_STREET;
        else if (leftSideStreetsNumber == 2 && rightSideStreetsNumber == 2) return Constants.POOL_TWO_TO_TWO_STREET;
        else if (leftSideStreetsNumber == 1 && rightSideStreetsNumber == 2) return Constants.POOL_ONE_TO_TWO_STREET;
        else return Constants.POOL_ONE_TO_ONE_STREET;
    }

}
