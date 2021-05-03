using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Vector3 pathOffset;
    public float velocity;

    private Quaternion currentQuaternionRotation;
    private float dstTravelledToBeReady;

    public Transform target;
    public float dstTravelled { get; set; }
    public System.Random rand { get; set; }
    public bool isReadyToInstanceObstacle { get; set; }

    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        dstTravelledToBeReady = 10;
        isReadyToInstanceObstacle = false;
        rand = new System.Random(); ;
    }

    public int MyProdstTravelledperty { get; set; }

    void Update()
    {
        if (velocity > 0)
        {
            if (target != null)
            {
                dstTravelled += velocity * Time.deltaTime;
                currentQuaternionRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, currentQuaternionRotation, velocity * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, target.position, velocity * Time.deltaTime);
                if(dstTravelled > dstTravelledToBeReady) isReadyToInstanceObstacle = true;
            }
            else isReadyToInstanceObstacle = false;
        }
    }

    public void ReSetInstanceTravelledToBeReady()
    {
        dstTravelledToBeReady= dstTravelled + rand.Next(15, 30);
        isReadyToInstanceObstacle = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Equals(other.gameObject.tag, "WayPoint") && (other.gameObject.transform == target || target == null))
        {
            target = other.gameObject.GetComponent<WayPoint>().nextWayPoint;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Equals(other.gameObject.tag, "WayPoint") && (other.gameObject.transform != null))
        {
            target = other.gameObject.GetComponent<WayPoint>().nextWayPoint;
        }
    }

}
