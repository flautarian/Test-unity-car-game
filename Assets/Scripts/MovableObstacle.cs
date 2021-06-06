using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacle: Obstacle
{
    // Start is called before the first frame update

    public float velocity;
    public float penalty;
    private bool automaticDriving = true;
    public bool isReverseObstacle;
    private Quaternion currentQuaternionRotation;
    public Transform target;

    internal void changeToNextPoint(Transform nextWayPoint)
    {
        target = nextWayPoint;
    }

    public Vector3 localPosition;

    void Start()
    {
    }

    void Update()
    {
        if(velocity > 0)
        {
            moveWheels(velocity);
            if (automaticDriving)
            {
                if (target != null)
                {
                    currentQuaternionRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, currentQuaternionRotation, velocity * Time.deltaTime);
                    transform.position = Vector3.MoveTowards(transform.position, target.position, velocity * Time.deltaTime);
                }
                else Destroy(this.gameObject);
            }
            /*else
            {
                GetComponent<Transform>().Translate(Vector3.forward * Time.deltaTime * velocity);
                if (transform.position.y < -1f) Destroy(this.gameObject);
            }*/
        }
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
        if (Equals(other.gameObject.tag, "WayPoint") && other.gameObject.transform == target)
        {
            WayPoint wayPoint = other.gameObject.GetComponent<WayPoint>();
            target = wayPoint.nextWayPoint;
        }
    }

    public override void SetPositioAndTargetFromSpawner(Spawner spawner)
    {
        if (spawner.target != null)
        {
            target = spawner.lastTarget != null ? spawner.lastTarget : spawner.target;
            transform.position = spawner.transform.position;
            transform.LookAt(target);
        }
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
