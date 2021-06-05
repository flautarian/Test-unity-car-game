using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObstacle : Obstacle
{
    public Vector3 localPosition;

    public bool rigidBodySlept;

    void Start()
    {
        if (rigidBodySlept) GetComponent<Rigidbody>().Sleep();
    }

    void Update()
    {
        if (transform.position.y < 0f) Destroy(this.gameObject);
    }

    public override void SetPositioAndTargetFromSpawner(Spawner spawner)
    {
        if (spawner.target != null)
        {
            transform.position = spawner.transform.position + spawner.sidewalkOffset;
            transform.LookAt(spawner.transform);
        }
    }

    private void OnCollisionStay(Collision c)
    {
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
