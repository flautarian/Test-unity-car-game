using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacle: Obstacle
{
    // Start is called before the first frame update

    public float velocity;
    private bool automaticDriving = true, isOtherCarInFront = false;
    private MovableObstacleBumber frontalBumper;
    public bool isReverseObstacle;
    public int streetNumber = 0;
    private Quaternion currentQuaternionRotation;
    public WayPoint actualWaypoint;
    public Transform vehicleTarget;

    internal void changeToNextPoint(WayPoint nextWayPoint)
    {
        actualWaypoint = nextWayPoint;
    }

    private void Awake()
    {
        
    }

    public Vector3 localPosition;

    void Start()
    {
        frontalBumper = GetComponentInChildren<MovableObstacleBumber>();
    }

    void Update()
    {
        tryLockTargetIfNotLocked();
        var vel = checkVelocity();
        if(vel > 0)
        {
            moveWheels(vel);
            if (automaticDriving)
            {
                if (vehicleTarget != null && vehicleTarget.transform.position - transform.position != Vector3.zero)
                {
                    currentQuaternionRotation = Quaternion.LookRotation(vehicleTarget.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, currentQuaternionRotation, vel * Time.deltaTime);
                    transform.position = Vector3.MoveTowards(transform.position, vehicleTarget.position, vel * Time.deltaTime);
                }
                if(transform.position.y < -100)
                    this.gameObject.SetActive(false);
            }
        }
    }

    private float checkVelocity()
    {
        if (actualWaypoint != null)
        {
            if (isVehicleLockedWaypoint())
            {
                return frontalBumper != null && frontalBumper.bumpDetected ? frontalBumper.velocityOfFrontVehicle : velocity;
            }
            return 0;
        }
        else return velocity;
    }

    private bool isVehicleLockedWaypoint()
    {
        if (actualWaypoint != null && actualWaypoint.isAPreludeOfIncorporation())
        {
            WayPoint wp = actualWaypoint.nextWayPoint[0].GetComponent<WayPoint>();
            if(wp != null)
            {
                return wp.isOccuped && this.gameObject.Equals(wp.vehicleLockingWaypoint);
            }
        }       
        return true;
    }

    private void tryLockTargetIfNotLocked()
    {
        if(actualWaypoint != null)
        {
            if (!actualWaypoint.isAPreludeOfIncorporation()) return;
            if (!isVehicleLockedWaypoint())
            {
                tryLockIncorporation();
            }
        }
    }

    private bool tryLockIncorporation()
    {
        return actualWaypoint.tryLockIncorporation(this.gameObject);
    }

    void LateUpdate()
    {
        if (!GetComponent<Animation>().isPlaying) return;
        transform.localPosition += localPosition;
    }
    private void moveWheels(float velocity)
    {
        for(int i =0; i < this.transform.childCount; i++)
        {
            if(this.transform.GetChild(i).name.Contains("wheel"))
                this.transform.GetChild(i).Rotate(Vector3.right * Time.deltaTime * velocity*50, Space.Self);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Equals(other.gameObject.tag, "WayPoint"))
        {
            WayPoint wayPoint = other.gameObject.GetComponent<WayPoint>();
            if (isSameWayPointThanVehicleTarget(wayPoint))
            {
                assignNewVehicleTarget(wayPoint);
                if (actualWaypoint != null && actualWaypoint.isAnIncorporation()) actualWaypoint.unlockWaypoint();
                actualWaypoint = wayPoint;
                actualWaypoint.tryLockIncorporation(this.gameObject);
            }

        }
    }

    private bool isSameWayPointThanVehicleTarget(WayPoint wayPoint)
    {
        if(vehicleTarget != null)
        {
            GameObject targetWP = vehicleTarget.gameObject;
            WayPoint wp = targetWP.GetComponent<WayPoint>();
            return wp.Equals(wayPoint);
        }
        return false;
    }

    private void assignNewVehicleTarget(WayPoint wayPoint)
    {
        if (wayPoint.nextWayPoint != null && wayPoint.nextWayPoint.Count > 0)
        {
            if (wayPoint.nextWayPoint.Count > streetNumber) vehicleTarget = wayPoint.nextWayPoint[streetNumber];
            else vehicleTarget = wayPoint.nextWayPoint[0];
        }
        // end of road reached, deactivating obstacle
        else gameObject.SetActive(false);
    }

    public override void SetPositioAndTargetFromSpawner(Spawner spawner)
    {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if(GetComponent<Animation>() != null && GetComponent<Animation>().isPlaying) GetComponent<Animation>().Stop();
            vehicleTarget = spawner.lastTarget != null ? spawner.lastTarget : spawner.target;
            transform.position = spawner.transform.position;
            transform.LookAt(vehicleTarget);
    }

    public override void Collide(Transform c)
    {
        MovableCollition(c);
    }

    void OnCollisionEnter(Collision c)
    {
        MovableCollition(c.transform);
    }

    private void MovableCollition(Transform c)
    {
        // If the object we hit is the enemy
        if (Equals(c.gameObject.tag, "Player"))
        {
            // start explode animation and disable path follow
            automaticDriving = false;
            // how much the character should be knocked back
            var magnitude = 5500;
            // calculate force vector
            var force = c.forward;
            force.y += 5;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            //GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude);
            GetComponent<Animation>().Play();
        }
    }
}
