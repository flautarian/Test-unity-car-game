using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorCarreteras : MonoBehaviour
{
    public List<GameObject> callesPorRecorrer;
    public List<GameObject> beredaObstaculos;
    public List<GameObject> calleObstaculos;
    public List<GameObject> beredaPowerUps;
    public List<GameObject> callePowerUps;
    
    public float timeLapseForDeployPowerUp;
    public PathCreation.PathCreator mainPath;
    private PathCreation.PathCreator mainCounterPath;
    private bool canDeployPowerUps = true;

    public GameObject calle0;
    public bool inicioJuego = false;
    public bool juegoTerminado;
    
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        agregarCalleInicioJuego();
    }

    // Update is called once per frame
    void Update()
    {
        if(inicioJuego) inicializarJuego();
    }

    void inicializarJuego()
    {
        callesPorRecorrer = new List<GameObject>(GameObject.FindGameObjectsWithTag("Calle"));
        enableTimer();
    }

    internal void detonateAllObstacles()
    {
        throw new NotImplementedException();
    }

    public void ciclarCalle(GameObject calle)
    {
        //Debug.Log("ciclando calle");
        agregarCalleAlFinal(1);
        callesPorRecorrer.Remove(calle);
        StartCoroutine(EsperarYDestruirCalle(2.0f, calle));
    }

    private IEnumerator EsperarYDestruirCalle(float timer, GameObject calle)
    {
        yield return new WaitForSeconds(timer);
        //if (mainPath != null) mainPath.bezierPath.DeleteSegment(0);
        if (mainCounterPath != null) mainCounterPath.bezierPath.DeleteSegment(0);
        Destroy(calle);
    }

    void agregarCalleInicioJuego()
    {
        agregarCalleAlFinal(5);
    }

    void agregarCalleAlFinal(int times)
    {
        for (int i = 0; i < times; i++) { 
            GameObject nuevaCalle = Instantiate(calle0);
            nuevaCalle.transform.parent = this.transform;
            if (nuevaCalle != null && callesPorRecorrer.Count > 0)
            {
                GameObject lastCalle = callesPorRecorrer[callesPorRecorrer.Count-1];
                Vector3 pos = lastCalle.transform.position;
                pos.z += getHeightOfCalle(callesPorRecorrer.Count - 1);
                nuevaCalle.GetComponent<Transform>().position = pos;
                procesarNuevosObstaculos(nuevaCalle);
            }
            callesPorRecorrer.Add(nuevaCalle);
            UpdatePaths(nuevaCalle);
        }
    }

    private void UpdatePaths(GameObject nuevaCalle)
    {
        PathCreation.PathCreator pathB = nuevaCalle.GetComponent<Calle>().path;
        pathB.transform.parent = this.transform;
        if (System.Object.Equals(this.mainPath, null)){
            this.mainPath = Instantiate(pathB);
            this.mainPath.InitializeEditorData(false);
            this.mainPath.transform.position = pathB.transform.position;
            this.mainPath.transform.rotation = pathB.transform.rotation;
            this.mainPath.transform.parent = this.transform;
            this.mainPath.name = "mainPath";
        }
        addSegmentsOfPathBToPathA(this.mainPath, pathB);
        Destroy(pathB.gameObject);

        PathCreation.PathCreator pathBCounter = nuevaCalle.GetComponent<Calle>().counterPath;
        pathBCounter.transform.parent = this.transform;
        if (System.Object.Equals(this.mainCounterPath, null)){
            this.mainCounterPath = Instantiate(pathBCounter);
            this.mainCounterPath.InitializeEditorData(false);
            this.mainCounterPath.transform.position = pathBCounter.transform.position;
            this.mainCounterPath.transform.rotation = pathBCounter.transform.rotation;
            this.mainCounterPath.transform.parent = this.transform;
            this.mainCounterPath.name = "mainCounterPath";
        }
        addSegmentsOfPathBToPathA(this.mainCounterPath, pathBCounter);
        Destroy(pathBCounter.gameObject);
    }

    private void addSegmentsOfPathBToPathA(PathCreation.PathCreator pathA, PathCreation.PathCreator pathB)
    {
        joinPath(pathA, pathB);
        pathA.bezierPath.NotifyPathModified();
        //pathA.EditorData.bezierOrVertexPathModified += pathA.TriggerPathUpdate;
        //pathA.EditorData.bezierOrVertexPathModified -= pathA.TriggerPathUpdate;
        //pathA.path.UpdateTransform(pathA.transform);
    }

    private void joinPath(PathCreation.PathCreator pathA, PathCreation.PathCreator pathB)
    {
        for (int i = 0; i < pathB.bezierPath.NumSegments; i++)
        {
            Vector3 vec = pathB.bezierPath.GetPointsInSegment(i)[0];
            vec.z += pathB.transform.position.z;
            pathA.bezierPath.AddSegmentToEnd(vec);
        }
    }

    private void procesarNuevosObstaculos(GameObject nuevaCalle)
    {
        var random = new System.Random();
        //processNewObstacles(nuevaCalle, random, beredaObstaculos, beredaPowerUps, "beredaSpawnPoint", true, 6 - GameObject.FindGameObjectsWithTag("BeredaObstaculo").Length);
        processNewObstacles(nuevaCalle, random, calleObstaculos, callePowerUps, "streetSpawnPoint", false, 4 - GameObject.FindGameObjectsWithTag("StreetObstaculo").Length);
    }

    private void processNewObstacles(GameObject calle, System.Random random, List<GameObject> obstaclesList, List<GameObject> powerUpsList, String spawnNameTag, bool enlazarConPadre, int obstaclesToPlace)
    {
        if(obstaclesList.Count > 0)
        {
            GameObject[] listaSpawnPointsObstaculos = GameObject.FindGameObjectsWithTag(spawnNameTag);
            foreach (GameObject obs in listaSpawnPointsObstaculos)
            {
                if(obstaclesToPlace > 0 && !obs.GetComponent<Spawner>().spawnIsOverlappingOtherCollitions())
                {
                    int randNum = random.Next(10);
                    if (randNum > 5)
                    {
                        GameObject obstaculoGO = Instantiate(obstaclesList[random.Next(obstaclesList.Count)]);
                        obstaculoGO.GetComponent<Obstaculo>().pathCreator = this.mainPath;
                        obstaculoGO.GetComponent<Obstaculo>().updatePathChangeFunction();
                        obstaculoGO.GetComponent<Transform>().position = this.mainPath.path.GetClosestPointOnPath(obs.transform.position);
                        if(enlazarConPadre) obstaculoGO.transform.parent = calle.transform;
                        obstaclesToPlace--;
                    }
                    else if(randNum > 3 && canDeployPowerUps)
                    {
                        GameObject powerUpGO = Instantiate(powerUpsList[random.Next(powerUpsList.Count)]);
                        powerUpGO.GetComponent<ParticleSystem>().Play();
                        powerUpGO.GetComponent<Transform>().position = obs.transform.position;
                        powerUpGO.transform.parent = calle.transform;
                        restitutePowerUp();
                    }
                }
                //Destroy(obs);
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
        return callesPorRecorrer[pos].GetComponent<BoxCollider>().size.z;
    }

    private float getWidthOfCalle(int pos)
    {
        return callesPorRecorrer[pos].GetComponent<SpriteRenderer>().size.y;
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
