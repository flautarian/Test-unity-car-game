using PathCreation;
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
    public List<GameObject> beredaPowerUps;
    public List<GameObject> callePowerUps;
    public int lvl = 0;

    public float timeLapseForDeployPowerUp;
    private PathCreation.PathCreator mainPath;
    private PathCreation.PathCreator mainCounterPath;
    public GameObject streetSpawnPointRight;
    public GameObject streetSpawnPointLeft;
    public GameObject sidewalkSpawnPointRight;
    public GameObject sidewalkSpawnPointLeft;
    private bool canDeployPowerUps = true;

    public bool inicioJuego = false;
    public bool juegoTerminado;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStreetsOfGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(inicioJuego) inicializarJuego();
        if (!juegoTerminado)
        {
            iterateSpawners();
        }
    }
    void InitializeStreetsOfGame()
    {
        InitializeFirstStreet();
        AddStreetsToRemaining(2);
    }

    private void InitializeFirstStreet()
    {
        streetsRemaining = new List<GameObject>();
        GameObject street0 = Instantiate(lvl0Roads[0]);
        street0.transform.parent = this.transform;
        streetsRemaining.Add(street0);
        InitializeWaypointOfSpawners(street0.GetComponent<Calle>());
    }

    private void iterateSpawners()
    {
        //iterateSpawner(streetSpawnPointRight);
        //iterateSpawner(streetSpawnPointLeft);
        //iterateSpawner(sidewalkSpawnPointLeft);
       //iterateSpawner(sidewalkSpawnPointRight);
    }

    void inicializarJuego()
    {
        enableTimer();
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
            if (nuevaCalle != null && streetsRemaining.Count > 0)
            {
                GameObject lastCalle = streetsRemaining[streetsRemaining.Count-1];
                Vector3 pos = lastCalle.transform.position;
                pos.z += getHeightOfCalle(streetsRemaining.Count - 1) -1;
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
        WayPointManager wayPointManager= nuevaCalle.GetComponent<Calle>().waypointManager;
        //wayPointManager.transform.parent = this.transform;
        lastStreet.waypointManager.lastWayPoint.nextWayPoint = wayPointManager.firstWayPoint.transform;
    }

    private void InitializeWaypointOfSpawners(Calle calleInicial)
    {
        if(streetSpawnPointRight != null) streetSpawnPointRight.GetComponent<Spawner>().target = calleInicial.waypointManager.lastWayPoint.transform;
        if(sidewalkSpawnPointRight != null) sidewalkSpawnPointRight.GetComponent<Spawner>().target = calleInicial.waypointManager.lastWayPoint.transform;
    }


    private bool addSegmentsOfPathBToPathA(PathCreation.PathCreator pathA, PathCreation.PathCreator pathB)
    {
        try
        {
            for (int i = 0; i < pathB.bezierPath.NumSegments; i++)
            {
                Vector3 vec = pathB.bezierPath.GetPointsInSegment(i)[0];
                vec.z += pathB.transform.position.z;
                pathA.bezierPath.AddSegmentToEnd(vec);
            }
            return true;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
        return false;
        
    }

    private void iterateSpawner(GameObject spawnPoint)
    {
        Spawner spawner = spawnPoint.GetComponent<Spawner>();
        if(spawner != null && spawner.isReadyToInstanceObstacle)
        {
                GameObject obstaculoGO = null;
                if (spawnPoint.tag.Equals("streetSpawnPoint")) 
                    obstaculoGO = Instantiate(calleObstaculos[spawner.rand.Next(calleObstaculos.Count)]);
                else 
                    obstaculoGO = Instantiate(beredaObstaculos[spawner.rand.Next(beredaObstaculos.Count)]);
                obstaculoGO.GetComponent<Obstacle>().SetPathFromSpawner(spawner);
                spawner.ReSetInstanceTravelledToBeReady();
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

    private void enableTimer()
    {
        GameObject timer = GameObject.FindGameObjectWithTag("Timer");
        if (timer != null)
            timer.GetComponent<Timer>().startCountTime();
    }

    public void endGame()
    {
        inicioJuego = false;
    }


}
