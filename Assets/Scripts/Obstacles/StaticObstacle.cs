﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObstacle : Obstacle
{
    public bool rigidBodySlept;

    private Animation obstacleAnimation;

    void Start()
    {
        if (rigidBody != null) rigidBody.Sleep();
        if (GetComponent<Animation>() != null) obstacleAnimation = GetComponent<Animation>();
    }


    public override void SetPositioAndTargetFromSpawner(Spawner spawner)
    {
        if (spawner.target != null)
        {
            if (rigidBody != null) rigidBody.velocity = Vector3.zero;
            if (obstacleAnimation != null && obstacleAnimation.isPlaying) obstacleAnimation.Stop();
            transform.position = spawner.transform.position + spawner.sidewalkOffset;
            transform.LookAt(spawner.transform);
            if (spawner.orientation == SpawnerOrientation.LEFT) transform.rotation = new Quaternion(0f, 90f, 0f, 0f);
            else transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
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
        if (Equals(c.gameObject.tag, Constants.GO_TAG_PLAYER) || Equals(c.gameObject.tag, Constants.GO_TAG_PLAYER_PART))
        {
            if (rigidBody != null)
            {
                // start explode animation and disable path follow
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
            }
            // start explode animation and disable path follow
            if (animator != null)
            {
                animator.SetBool(Constants.ANIMATION_NAME_HIT_BOOL, true);
            }
        }
        else if (rigidBody != null && rigidBodySlept && !rigidBody.IsSleeping())
        {
            rigidBody.Sleep();
        }

    }

}