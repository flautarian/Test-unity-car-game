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
        StaticCollition(c.transform);
    }
    void OnCollisionEnter(Collision c)
    {
        StaticCollition(c.transform);
    }

    public override void Collide(Transform c)
    {
        StaticCollition(c);
    }

    private void StaticCollition(Transform c)
    {
        // If the object we hit is the enemy
        if (Equals(c.gameObject.tag, "Player") || Equals(c.gameObject.tag, "PlayerPart"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            // how much the character should be knocked back
            var magnitude = 2500;
            // calculate force vector
            var force = c.forward;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            //GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude);
            // start explode animation and disable path follow
        }
    }
}
