using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacle : Obstacle
{
    // Start is called before the first frame update

    public bool automaticDriving = true, frontalCarDetected = false;
    private Quaternion currentQuaternionRotation;
    public float velocity, frontalCarVelocity, vel;
    public bool isReverseObstacle;
    public int streetNumber = 0;
    public WayPoint actualWaypoint;
    public Transform vehicleTarget;

    internal void changeToNextPoint(WayPoint nextWayPoint)
    {
        actualWaypoint = nextWayPoint;
    }

    void Update()
    {
        tryLockTargetIfNotLocked();
        vel = checkVelocity();
        moveWheels(vel);
        if (vel > 0)
        {
            if (automaticDriving)
            {
                if (vehicleTarget != null && vehicleTarget.transform.position - transform.position != Vector3.zero)
                {
                    currentQuaternionRotation = Quaternion.LookRotation(vehicleTarget.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, currentQuaternionRotation, vel * Time.deltaTime);
                    transform.position = Vector3.MoveTowards(transform.position, vehicleTarget.position, vel * Time.deltaTime);
                }
                if (transform.position.y < -100)
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
                return frontalCarDetected ? frontalCarVelocity : velocity;
            }
            return 0;
        }
        else return frontalCarDetected ? frontalCarVelocity : velocity;
    }

    private bool isVehicleLockedWaypoint()
    {
        if (actualWaypoint != null && actualWaypoint.isAPreludeOfIncorporation())
        {
            WayPoint wp = actualWaypoint.nextWayPoint[0].GetComponent<WayPoint>();
            if (wp != null)
            {
                return wp.isOccuped && this.gameObject.Equals(wp.vehicleLockingWaypoint);
            }
        }
        return true;
    }

    private void tryLockTargetIfNotLocked()
    {
        if (actualWaypoint != null)
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

    private void moveWheels(float velocity)
    {
        animator.SetFloat(Constants.ANIMATION_MOVABLE_OBSTACLE_VELOCITY_PARAM, velocity > 0 ? 1 : 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        manageNextWayPoint(other);
    }

    private void OnTriggerStay(Collider other)
    {
        manageNextWayPoint(other);
    }

    private void manageNextWayPoint(Collider other)
    {
        if (Equals(other.gameObject.tag, Constants.GO_TAG_WAYPOINT))
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
        if (vehicleTarget != null)
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
            if (wayPoint.nextWayPoint.Count > streetNumber) vehicleTarget = wayPoint.nextWayPoint[streetNumber].transform;
            else vehicleTarget = wayPoint.nextWayPoint[0].transform;
        }
        // end of road reached, deactivating obstacle
        else gameObject.SetActive(false);
    }

    public override void SetPositioAndTargetFromSpawner(Spawner spawner)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
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
        if (Equals(c.gameObject.tag, Constants.GO_TAG_PLAYER) ||
            Equals(c.gameObject.tag, Constants.GO_TAG_PLAYER_PART))
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
            GetComponent<Rigidbody>().AddForce(force * magnitude);
        }
    }

    public void frontalCarBumperDetected(float velocityOfCar)
    {
        frontalCarVelocity = velocityOfCar;
        frontalCarDetected = true;
    }

    public void frontalCarBumperHidden()
    {
        frontalCarDetected = false;
    }
}
