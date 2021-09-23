using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float velocity;
    private bool startedSpawnerMovement = false;
    private Quaternion currentQuaternionRotation;
    public float dstTravelledToInstanceMovable;
    public float dstTravelledToInstanceStatic;
    private bool spawnerArrivedToTarget;

    public Vector3 sidewalkOffset;
    public Transform target;
    public Transform lastTarget;
    private WayPoint nextWayPoint;
    private Transform obstaclesContainer;
    public SpawnerOrientation orientation;
    private float lastTimeManagedSpawner;

    public float dstTravelled { get; set; }
    public System.Random rand { get; set; }

    public bool isReadyToInstanceMovableObstacle { get; set; }
    public bool isReadyToInstanceStaticObstacle { get; set; }

    public bool canInstanceMovableObstacle { get; set; }
    public bool canInstanceStaticObstacle { get; set; }

    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        dstTravelledToInstanceMovable = 10;
        dstTravelledToInstanceStatic = 25;
        isReadyToInstanceMovableObstacle = false;
        rand = new System.Random();
        var obsgo = GameObject.FindGameObjectWithTag(Constants.GO_TAG_OBSTACLE_CONTAINER);
        if (obsgo != null) obstaclesContainer = obsgo.transform;
        lastTimeManagedSpawner = Time.time;
    }

    public int MyProdstTravelledperty { get; set; }

    private void Update()
    {
        GetComponent<MeshRenderer>().material.color = isReadyToInstanceMovableObstacle ? Color.green : Color.red;
        if (Time.time - lastTimeManagedSpawner >= 0.2f) manageSpawner();
        if (target != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, currentQuaternionRotation, velocity * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, target.position, velocity * Time.deltaTime);
        }
    }

    private void manageSpawner()
    {
        lastTimeManagedSpawner = Time.time;
        if (velocity > 0 || orientation == SpawnerOrientation.LEFT)
        {
            if (target != null)
            {
                dstTravelled += velocity * Time.deltaTime;
                spawnerArrivedToTarget = (target.position - transform.position) == Vector3.zero;
                if (!spawnerArrivedToTarget)
                {
                    currentQuaternionRotation = Quaternion.LookRotation(target.position - transform.position);
                    if (dstTravelled > dstTravelledToInstanceMovable) isReadyToInstanceMovableObstacle = true;
                    if (dstTravelled > dstTravelledToInstanceStatic) isReadyToInstanceStaticObstacle = true;
                }
            }
            else if (!startedSpawnerMovement)
            {
                Calle calle = GlobalVariables.Instance.lastCalle;
                if (calle != null)
                {
                    WayPointManager wpm = calle.waypointManager;
                    if (wpm != null)
                    {
                        Transform firstTarget = SpawnerOrientation.RIGHT.Equals(orientation) ? wpm.firstWayPoint[0].transform : wpm.lastWayReversalPoint[0].transform;
                        if (firstTarget != null)
                        {
                            transform.LookAt(firstTarget);
                            transform.position = firstTarget.position;
                        }
                    }
                }
                startedSpawnerMovement = false;
            }
            CheckAndDeployObstacles();
        }
    }

    public void ReSetMovableSpawnerTrigger()
    {
        dstTravelledToInstanceMovable = dstTravelled + rand.Next(1, 5);
        isReadyToInstanceMovableObstacle = false;
    }

    public void ReSetStaticSpawnerTrigger()
    {
        dstTravelledToInstanceStatic = dstTravelled + rand.Next(15, 30);
        isReadyToInstanceStaticObstacle = false;
    }

    private void updateTarget()
    {
        velocity = GlobalVariables.Instance.playerCurrentVelocity > 20 ? GlobalVariables.Instance.playerCurrentVelocity * 1.2f : 20;
        if (nextWayPoint.isReverse)
        {
            if (nextWayPoint.previousWayPoint != null && nextWayPoint.previousWayPoint.Count > 0 && nextWayPoint.previousWayPoint[0] != target)
            {
                lastTarget = target;
                target = nextWayPoint.previousWayPoint[0].transform;
            }
        }
        else if (nextWayPoint.nextWayPoint != null && nextWayPoint.nextWayPoint.Count > 0 && nextWayPoint.nextWayPoint[0] != target)
        {
            lastTarget = target;
            target = nextWayPoint.nextWayPoint[0].transform;
        }

    }

    private void CheckAndDeployObstacles()
    {
        if (target != null)
        {
            if (isReadyToInstanceMovableObstacle)
            {
                GameObject obstaculoGO = PoolManager.Instance.SpawnFromPool(Constants.POOL_STREET_OBSTACLE, transform.position, transform.rotation, obstaclesContainer);
                if (obstaculoGO != null)
                {
                    obstaculoGO.GetComponent<Obstacle>().SetPositioAndTargetFromSpawner(this);
                    ReSetMovableSpawnerTrigger();
                }
                //else Debug.Log("MEK, sin vehiculos en el pool!!");
            }

            if (isReadyToInstanceStaticObstacle)
            {
                GameObject obstaculoGO = PoolManager.Instance.SpawnFromPool(Constants.POOL_BEREDA_OBSTACLE, transform.position, transform.rotation, obstaclesContainer);
                if (obstaculoGO != null)
                {
                    obstaculoGO.GetComponent<Obstacle>().SetPositioAndTargetFromSpawner(this);
                    ReSetStaticSpawnerTrigger();
                }
                //else Debug.Log("MEK, sin obstaculos estaticos en el pool!!");
            }
        }
    }

    void OnCollisionEnter(Collision c)
    {
        checkSpawnerCollidersWithObstables(c);
    }

    void OnCollisionStay(Collision c)
    {
        checkSpawnerCollidersWithObstables(c);
    }

    private void checkSpawnerCollidersWithObstables(Collision c)
    {
        if (Equals(c.gameObject.tag, Constants.POOL_STREET_OBSTACLE) && orientation != SpawnerOrientation.LEFT)
        {
            isReadyToInstanceMovableObstacle = false;
        }
        else if (Equals(c.gameObject.tag, Constants.POOL_BEREDA_OBSTACLE))
        {
            isReadyToInstanceStaticObstacle = false;
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        ManageSpawnerTriggerCollider(c);
    }
    private void OnTriggerStay(Collider c)
    {
        ManageSpawnerTriggerCollider(c);
    }

    private void ManageSpawnerTriggerCollider(Collider c)
    {
        if (Equals(c.gameObject.tag, Constants.GO_TAG_WAYPOINT) && (c.gameObject.transform == target || target == null))
        {
            nextWayPoint = c.gameObject.GetComponent<WayPoint>();
            if (nextWayPoint != null && nextWayPoint.isReverse.Equals(orientation.Equals(SpawnerOrientation.LEFT))) updateTarget();
        }
    }

}
