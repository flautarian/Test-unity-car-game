using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObstacle : Obstacle
{
    public Vector3 localPosition;

    public bool rigidBodySlept;

    void Start()
    {
        localPosition = transform.localPosition;
        GetComponent<Rigidbody>().Sleep();
    }


    public override void SetPositioAndTargetFromSpawner(Spawner spawner)
    {
        if (spawner.target != null)
        {
            rigidBody.velocity = Vector3.zero;
            if ( GetComponent<Animation>() != null && GetComponent<Animation>().isPlaying) 
                GetComponent<Animation>().Stop();
            transform.position = spawner.transform.position + spawner.sidewalkOffset;
            transform.LookAt(spawner.transform);
            if(spawner.orientation == SpawnerOrientation.LEFT)transform.rotation = new Quaternion(0f, 90f, 0f, 0f);
            else transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
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
            // start explode animation and disable path follow
            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_BOOM, transform.position);
            rigidBodySlept = false;
            rigidBody.isKinematic = false;
            // how much the character should be knocked back
            var magnitude = 2500;
            // calculate force vector
            var force = c.forward;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            //GetComponent<MeshRenderer>().enabled = false;
            rigidBody.AddForce(force * magnitude);
            // start explode animation and disable path follow
        }
        else if (rigidBodySlept && !rigidBody.IsSleeping())
        {
            rigidBody.Sleep();
        }
    }

}
