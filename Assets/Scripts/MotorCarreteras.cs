using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorCarreteras : MonoBehaviour
{
    private List<GameObject> streetsRemaining;
    public List<GameObject> lvl0Roads;
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
        AddStreetsToRemaining(2);
        InitializeWaypointOfSpawners(streetsRemaining[streetsRemaining.Count-1].GetComponent<Calle>());
    }

    private void InitializeFirstStreet()
    {
        streetsRemaining = new List<GameObject>();
        GameObject street0 = Instantiate(lvl0Roads[0]);
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
            GameObject nuevaCalle = Instantiate(GetNewRandomRoad());
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

    private GameObject GetNewRandomRoad()
    {
        switch (lvl)
        {
            case 0:
                return lvl0Roads[UnityEngine.Random.Range(0, lvl0Roads.Count)];
            default:
                return lvl0Roads[UnityEngine.Random.Range(0, lvl0Roads.Count)];
        }
    }

    private void UpdatePaths(GameObject nuevaCalle, GameObject lastCalle)
    {
        Calle lastStreet = lastCalle.GetComponent<Calle>();
        Calle newStreet = nuevaCalle.GetComponent<Calle>();
        lastStreet.waypointManager.lastWayPoint.nextWayPoint = newStreet.waypointManager.firstWayPoint.transform;
        newStreet.waypointManager.firstWayPoint.previousWayPoint = lastStreet.waypointManager.lastWayPoint.transform;
        newStreet.waypointManager.lastWayReversalPoint.nextWayPoint = lastStreet.waypointManager.firstWayReversalPoint.transform;
        lastStreet.waypointManager.firstWayReversalPoint.previousWayPoint = newStreet.waypointManager.lastWayReversalPoint.transform;
    }

    private void InitializeWaypointOfSpawners(Calle calleInicial)
    {
        if (streetSpawnPointRight != null) InitilizeWaypoint(streetSpawnPointRight, calleInicial.waypointManager.lastWayPoint.transform);
        if (streetSpawnPointLeft != null) InitilizeWaypoint(streetSpawnPointLeft, calleInicial.waypointManager.firstWayReversalPoint.transform);
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
