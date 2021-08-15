using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorCarreteras : MonoBehaviour
{
    private List<GameObject> streetsRemaining;
    public List<GameObject> oneToOneWayRoads;
    public List<GameObject> oneToTwoWayRoads;
    public List<GameObject> twoToOneWayRoads;
    public List<GameObject> twoToTwoWayRoads;
    public List<GameObject> beredaObstaculos;
    public List<GameObject> calleObstaculos;
    public List<GameObject> powerUps;
    public int lvl = 0;

    public float timeLapseForDeployPowerUp;
    public GameObject streetSpawnPointRight;
    public GameObject streetSpawnPointLeft;
    private bool canDeployPowerUps = true,
        gameStarted = false;


    // Start is called before the first frame update
    void Start()
    {
        InitializeStreetsOfGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            iterateSpawners();
        }
    }
    void InitializeStreetsOfGame()
    {
        InitializeFirstStreet();
        AddStreetsToRemaining(6);
        InitializeWaypointOfSpawners(streetsRemaining[streetsRemaining.Count-1].GetComponent<Calle>());
    }

    private void InitializeFirstStreet()
    {
        streetsRemaining = new List<GameObject>();
        GameObject street0 = Instantiate(oneToOneWayRoads[0]);
        street0.transform.parent = this.transform;
        streetsRemaining.Add(street0);
    }

    private void iterateSpawners()
    {
        IterateSpawner(streetSpawnPointRight);
        IterateSpawner(streetSpawnPointLeft);
    }

    internal void detonateAllObstacles()
    {
        throw new NotImplementedException();
    }

    public IEnumerator ciclarCalle(GameObject street)
    {
        yield return new WaitForEndOfFrame();
        AddStreetsToRemaining(1);
        streetsRemaining.Remove(street);
    }

    void AddStreetsToRemaining(int times)
    {
        for (int i = 0; i < times; i++) { 
            GameObject nuevaCalle = Instantiate(GetNewRandomRoad(streetsRemaining[streetsRemaining.Count - 1].GetComponent<Calle>()));
            nuevaCalle.transform.parent = this.transform;
            nuevaCalle.GetComponent<Calle>().motor = this;
            nuevaCalle.GetComponent<Calle>().initializePowerUps(this.powerUps);
            if (nuevaCalle != null && streetsRemaining.Count > 0)
            {
                GameObject lastCalle = streetsRemaining[streetsRemaining.Count-1];
                Vector3 pos = lastCalle.transform.position;
                pos.z += getHeightOfCalle(streetsRemaining.Count - 1) - 0.5f;
                nuevaCalle.GetComponent<Transform>().position = pos;
                UpdatePaths(nuevaCalle, lastCalle);
            }
            streetsRemaining.Add(nuevaCalle);
        }
    }

    private GameObject GetNewRandomRoad(Calle lastStreet)
    {
        WayPointManager wpLastStreet = lastStreet.waypointManager;
        if(wpLastStreet != null)
        {
            int leftSideStreetsNumber = wpLastStreet.lastWayReversalPoint.Count;
            int rightSideStreetsNumber = wpLastStreet.lastWayPoint.Count;
            if(leftSideStreetsNumber == 1 && rightSideStreetsNumber == 1) return oneToOneWayRoads[UnityEngine.Random.Range(0, oneToOneWayRoads.Count)];
            else if (leftSideStreetsNumber == 2 && rightSideStreetsNumber == 1) return twoToOneWayRoads[UnityEngine.Random.Range(0, twoToOneWayRoads.Count)];
            else if (leftSideStreetsNumber == 2 && rightSideStreetsNumber == 2) return twoToTwoWayRoads[UnityEngine.Random.Range(0, twoToTwoWayRoads.Count)];
            else if (leftSideStreetsNumber == 1 && rightSideStreetsNumber == 2) return oneToTwoWayRoads[UnityEngine.Random.Range(0, oneToTwoWayRoads.Count)];
        }
        return oneToOneWayRoads[UnityEngine.Random.Range(0, oneToOneWayRoads.Count)];
    }

    private void UpdatePaths(GameObject nuevaCalle, GameObject lastCalle)
    {
        Calle lastStreet = lastCalle.GetComponent<Calle>();
        Calle newStreet = nuevaCalle.GetComponent<Calle>();
        WayPointManager lastWpm = lastStreet.waypointManager;
        WayPointManager newWpm = newStreet.waypointManager;
        // interconnectem els carrils dels npcs entre l'ultima street i la nova que entra
        newWpm.addPreviousWayPoint(lastWpm.lastWayPoint);
        lastWpm.addToNextWayPoint(newWpm.firstWayPoint);
        lastWpm.addNextReversalWayPoint(newWpm.lastWayReversalPoint);
        newWpm.addPreviousReversalWayPoint(lastWpm.firstWayReversalPoint);
        
        /*lastStreet.waypointManager.lastWayPoint.nextWayPoint = newStreet.waypointManager.firstWayPoint.transform;
        newStreet.waypointManager.firstWayPoint.previousWayPoint = lastStreet.waypointManager.lastWayPoint.transform;
        newStreet.waypointManager.lastWayReversalPoint.nextWayPoint = lastStreet.waypointManager.firstWayReversalPoint.transform;
        lastStreet.waypointManager.firstWayReversalPoint.previousWayPoint = newStreet.waypointManager.lastWayReversalPoint.transform;*/
    }

    private void InitializeWaypointOfSpawners(Calle calleInicial)
    {
        if (streetSpawnPointRight != null) InitilizeWaypoint(streetSpawnPointRight, calleInicial.waypointManager.lastWayPoint[0].transform);
        if (streetSpawnPointLeft != null) InitilizeWaypoint(streetSpawnPointLeft, calleInicial.waypointManager.firstWayReversalPoint[0].transform);
    }

    private void InitilizeWaypoint(GameObject spawner, Transform target)
    {
        spawner.GetComponent<Spawner>().target = target;
        spawner.transform.LookAt(target);
        spawner.transform.position = target.position;
    }

    private void IterateSpawner(GameObject spawnPoint)
    {
        Spawner spawner = spawnPoint.GetComponent<Spawner>();
        if(spawner != null)
        {
            if (spawner.isReadyToInstanceMovableObstacle)
            {
                GameObject obstaculoGO = Instantiate(calleObstaculos[spawner.rand.Next(calleObstaculos.Count)]);    
                obstaculoGO.GetComponent<Obstacle>().SetPositioAndTargetFromSpawner(spawner);
                spawner.ReSetMovableSpawnerTrigger();
            }
            else if (spawner.isReadyToInstanceStaticObstacle)
            {
                GameObject obstaculoGO = Instantiate(beredaObstaculos[spawner.rand.Next(beredaObstaculos.Count)]);
                obstaculoGO.GetComponent<Obstacle>().SetPositioAndTargetFromSpawner(spawner);
                spawner.ReSetStaticSpawnerTrigger();
            }
        }
    }

    private void restitutePowerUp()
    {
        StartCoroutine(resetPowerUpDeploy());
    }

    private IEnumerator resetPowerUpDeploy()
    {
        canDeployPowerUps = false;
        yield return new WaitForSeconds(timeLapseForDeployPowerUp);
        canDeployPowerUps = true;
    }

    private float getHeightOfCalle(int pos)
    {
        return streetsRemaining[pos].GetComponent<BoxCollider>().size.z;
    }

    private float getWidthOfCalle(int pos)
    {
        return streetsRemaining[pos].GetComponent<BoxCollider>().size.x;
    }

    public void startGame()
    {
        gameStarted = true;
    }

    public void startGameOver()
    {
        gameStarted = false;
    }

}
