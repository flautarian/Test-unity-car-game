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

    private void iterateSpawners()
    {
        iterateSpawner(streetSpawnPointRight);
        //iterateSpawner(streetSpawnPointLeft);
        iterateSpawner(sidewalkSpawnPointLeft);
        iterateSpawner(sidewalkSpawnPointRight);
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
        StartCoroutine(EsperarYDestruirCalle(2.0f, street));
    }

    private IEnumerator EsperarYDestruirCalle(float timer, GameObject calle)
    {
        yield return new WaitForSeconds(timer);
        if (calle != null)
        {
            streetsRemaining.Remove(calle);
            addSegmentsToDelete(calle.GetComponent<Calle>());
            Destroy(calle);
        }
    }

    private void addSegmentsToDelete(Calle calle)
    {
        this.mainCounterPath.bezierPath.numPointsToDelete = this.mainCounterPath.bezierPath.numPointsToDelete + calle.path.bezierPath.NumPoints;
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
        UpdatePaths(street0);
        streetsRemaining.Add(street0);
    }

    void AddStreetsToRemaining(int times)
    {
        for (int i = 0; i < times; i++) { 
            GameObject nuevaCalle = Instantiate(GetNewRandomRoad());
            nuevaCalle.transform.parent = this.transform;
            if (nuevaCalle != null && streetsRemaining.Count > 0)
            {
                GameObject lastCalle = streetsRemaining[streetsRemaining.Count-1];
                Vector3 pos = lastCalle.transform.position;
                pos.z += getHeightOfCalle(streetsRemaining.Count - 1) - 1;
                nuevaCalle.GetComponent<Transform>().position = pos;
            }
            streetsRemaining.Add(nuevaCalle);
            UpdatePaths(nuevaCalle);
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

    private void UpdatePaths(GameObject nuevaCalle)
    {
        PathCreation.PathCreator pathB = nuevaCalle.GetComponent<Calle>().path;
        pathB.transform.parent = this.transform;
        if (System.Object.Equals(this.mainPath, null))
        {
            this.mainPath = Instantiate(pathB);
            this.mainPath.InitializeEditorData(false);
            this.mainPath.transform.position = pathB.transform.position;
            this.mainPath.transform.rotation = pathB.transform.rotation;
            this.mainPath.transform.parent = this.transform;
            this.mainPath.name = "mainPath";
            resetPathOfSpawners();
        }
        else addSegmentsOfPathBToPathA(this.mainPath, pathB);
        
        Destroy(pathB.gameObject);
    }

    private void resetPathOfSpawners()
    {
        if(streetSpawnPointRight != null) streetSpawnPointRight.GetComponent<Spawner>().setPath(this.mainPath);
        if(sidewalkSpawnPointRight != null) sidewalkSpawnPointRight.GetComponent<Spawner>().setPath(this.mainPath);
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
            if(!spawner.spawnIsOverlappingOtherCollitions())
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
