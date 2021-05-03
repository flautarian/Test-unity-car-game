using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObstacle : Obstacle
{
    public Vector3 localPosition;

    void Start()
    {
        
    }

    void Update()
    {
        if (transform.position.y < 0f) Destroy(this.gameObject);
        //if (!GetComponent<Animation>().isPlaying) return;
        //transform.localPosition += localPosition;
    }

    public override void SetPathFromSpawner(Spawner spawner)
    {
        transform.parent = spawner.transform.parent;
        transform.position = spawner.transform.position;
    }

    void OnCollisionEnter(Collision c)
    {
        // If the object we hit is the enemy
        if (Equals(c.gameObject.tag, "Player"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            // how much the character should be knocked back
            var magnitude = 500;
            // calculate force vector
            var force = transform.position + Vector3.one;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            //GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude);
            // start explode animation and disable path follow
        }
    }
}
