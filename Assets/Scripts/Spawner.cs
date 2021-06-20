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

    public Vector3 sidewalkOffset;
    public Transform target;
    public Transform lastTarget;
    public SpawnerOrientation orientation;

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

    void Update()
    {
        GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[isReadyToInstanceMovableObstacle? 1 : 0];
        if (velocity > 0)
        {
            if (target != null && transform.position != Vector3.zero)
            {
                dstTravelled += velocity * Time.deltaTime;
                currentQuaternionRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, currentQuaternionRotation, velocity * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, target.position, velocity * Time.deltaTime);
                if (dstTravelled > dstTravelledToInstanceMovable) isReadyToInstanceMovableObstacle = true;
                if (dstTravelled > dstTravelledToInstanceStatic) isReadyToInstanceStaticObstacle = true;
            }
            else
            {
                isReadyToInstanceMovableObstacle = false;
                isReadyToInstanceStaticObstacle = false;
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
    private void OnTriggerEnter(Collider c)
    {
        if (Equals(c.gameObject.tag, "WayPoint") && (c.gameObject.transform == target || target == null))
        {
            updateTarget(c.gameObject.GetComponent<WayPoint>());
        }
    }

    private void updateTarget(WayPoint wayPoint)
    {
        if (wayPoint.isReverse)
        {
            if(wayPoint.previousWayPoint != null && wayPoint.previousWayPoint != target)
            {
                lastTarget = target;
                target = wayPoint.previousWayPoint;
            }
        }
        else if ( wayPoint.nextWayPoint != target)
            target = wayPoint.nextWayPoint;
    }

    void OnCollisionEnter(Collision c)
    {
        // If the object we hit is an obstacle
        if (Equals(c.gameObject.tag, "StreetObstaculo") || Equals(c.gameObject.tag, "BeredaObstaculo"))
        {
            isReadyToInstanceMovableObstacle = false;
        }
    }

    void OnCollisionExit(Collision c)
    {
        // If the object we hit is an obstacle
        if (Equals(c.gameObject.tag, "StreetObstaculo") || Equals(c.gameObject.tag, "BeredaObstaculo"))
        {
            isReadyToInstanceMovableObstacle = true;
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
