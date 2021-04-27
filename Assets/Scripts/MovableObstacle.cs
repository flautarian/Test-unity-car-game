using PathCreation;
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
    public PathCreator path;
    public Vector3 localPosition;
    public float dstTravelled = 0;

    public bool sense { get; set; } = true;
    void Start()
    {
    }
    private void OnPathChanged()
    {
        if (path != null)
        {
            if(path.bezierPath.GetPoint(0).z > transform.position.z) Destroy(this.gameObject);
            else dstTravelled = path.path.GetClosestDistanceAlongPath(transform.position);
        }
    }

    void Update()
    {
        if(velocity > 0)
        {
            dstTravelled += velocity * Time.deltaTime;
            moveWheels(velocity);
            if (automaticDriving)
            {
                if (path != null){
                    Vector3 pos = path.path.GetPointAtDistance(dstTravelled);
                    // desviamos segun el sentido
                    pos.x += sense ? 1.5f : -1.5f;
                    //no permitimos que los obstaculos que llegan al final vivan
                    if (pos.z < transform.position.z) Destroy(this.gameObject);
                    transform.position = pos;
                    Quaternion rot = path.path.GetRotationAtDistance(dstTravelled);
                    transform.rotation = rot;
                }
            }
            else GetComponent<Transform>().Translate(Vector3.forward * Time.deltaTime * velocity);
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

    public void OnDestroy()
    {
        if (path != null) path.pathUpdated -= OnPathChanged;
    }

    void OnCollisionEnter(Collision c)
    {
        // If the object we hit is the enemy
        if (Equals(c.gameObject.tag, "Player"))
        {
            // how much the character should be knocked back
            var magnitude = 5;
            // calculate force vector
            var force = transform.position - c.transform.position;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            //GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude);
            // start explode animation and disable path follow
            automaticDriving = false;
            GetComponent<Animation>().Play();
        }
    }

    public override void SetPathFromSpawner(Spawner spawner)
    {
        if (spawner.path != null)
        {
            path = spawner.path;
            transform.parent = spawner.transform.parent;
            transform.position = path.path.GetClosestPointOnPath(spawner.transform.position);
            sense = spawner.sense;
            dstTravelled = spawner.dstTravelled;
            path.pathUpdated += OnPathChanged;
        }
    }
}
