using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float velocity;
    private Quaternion currentQuaternionRotation;
    private float dstTravelledToInstanceMovable;
    private float dstTravelledToInstanceStatic;
    private bool spawnerArrivedToTarget;

    public Vector3 sidewalkOffset;
    public Transform target;
    public Transform lastTarget;
    public SpawnerOrientation orientation;
    public List<string> poolTagsOfMovableObstacles;
    public List<string> poolTagsOfStaticObstacles;

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
    }

    public int MyProdstTravelledperty { get; set; }

    private void FixedUpdate()
    {
        GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[isReadyToInstanceMovableObstacle? 1 : 0];
        if (velocity > 0)
        {
            if (target != null)
            {
                dstTravelled += velocity * Time.deltaTime;
                spawnerArrivedToTarget = (target.transform.position - transform.position) == Vector3.zero;
                if (!spawnerArrivedToTarget)
                {
                    currentQuaternionRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    if (dstTravelled > dstTravelledToInstanceMovable) isReadyToInstanceMovableObstacle = true;
                    if (dstTravelled > dstTravelledToInstanceStatic) isReadyToInstanceStaticObstacle = true;
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, currentQuaternionRotation, velocity * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, target.position, velocity * Time.deltaTime);
            }
        }
    }

    public void ReSetMovableSpawnerTrigger()
    {
        dstTravelledToInstanceMovable = dstTravelled + rand.Next(50, 75);
        isReadyToInstanceMovableObstacle = false;
    }

    public void ReSetStaticSpawnerTrigger()
    {
        dstTravelledToInstanceStatic = dstTravelled + rand.Next(100, 250);
        isReadyToInstanceStaticObstacle = false;
    }

    private void updateTarget(WayPoint wayPoint)
    {
        if (wayPoint.isReverse)
        {
            if(wayPoint.previousWayPoint != null && wayPoint.previousWayPoint.Count > 0 && wayPoint.previousWayPoint[0] != target)
            {
                lastTarget = target;
                target = wayPoint.previousWayPoint[0];
            }
        }
        else if (wayPoint.nextWayPoint != null && wayPoint.nextWayPoint.Count > 0 && wayPoint.nextWayPoint[0] != target )
        {
            lastTarget = target;
            target = wayPoint.nextWayPoint[0];
        }

        CheckAndDeployObstacles();
    }

    private void CheckAndDeployObstacles()
    {
        if (target != null)
        {
            if (isReadyToInstanceMovableObstacle)
            {
                GameObject obstaculoGO = PoolManager.Instance.SpawnFromPool("StreetObstaculo", transform.position, transform.rotation);
                if (obstaculoGO != null)
                {
                    obstaculoGO.GetComponent<Obstacle>().SetPositioAndTargetFromSpawner(this);
                    ReSetMovableSpawnerTrigger();
                }
            }

            if (isReadyToInstanceStaticObstacle)
            {
                GameObject obstaculoGO = PoolManager.Instance.SpawnFromPool("BeredaObstaculo", transform.position, transform.rotation); ;
                if (obstaculoGO != null)
                {
                    obstaculoGO.GetComponent<Obstacle>().SetPositioAndTargetFromSpawner(this);
                    ReSetStaticSpawnerTrigger();
                }
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

    void OnCollisionExit(Collision c)
    {
        checkSpawnerCollidersWithObstables(c);
    }

    private void checkSpawnerCollidersWithObstables(Collision c)
    {
        if (Equals(c.gameObject.tag, "StreetObstaculo") || Equals(c.gameObject.tag, "BeredaObstaculo"))
        {
            isReadyToInstanceMovableObstacle = false;
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (Equals(c.gameObject.tag, "WayPoint") && (c.gameObject.transform == target || target == null))
        {
            updateTarget(c.gameObject.GetComponent<WayPoint>());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Equals(other.gameObject.tag, "WayPoint"))
        {
            updateTarget(other.gameObject.GetComponent<WayPoint>());
        }
    }

}
