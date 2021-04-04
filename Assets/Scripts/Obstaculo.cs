﻿using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    // Start is called before the first frame update

    public float velocity;
    public float penalty;
    public PathCreator path;
    private GameObject player;
    private float dstTravelled = 0;
    void Start()
    {
        if (System.Object.Equals(player, null)) 
            player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SubscribePathChanges()
    {
        if (path != null)path.pathUpdated += OnPathChanged;
    }

    private void OnPathChanged()
    {
        if(path != null) dstTravelled = path.path.GetClosestDistanceAlongPath(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Transform>().Translate(Vector2.up * Time.deltaTime * velocity);

        if(velocity > 0)
        {
            dstTravelled += velocity * Time.deltaTime;
            moveWheels(velocity);
            if(path != null){
                transform.position = path.path.GetPointAtDistance(dstTravelled);
                Quaternion rot = path.path.GetRotationAtDistance(dstTravelled);
                transform.rotation = rot;
            }
            else GetComponent<Transform>().Translate(Vector3.forward * Time.deltaTime * velocity);
            if (player != null && getDistanceBetweenPlayerAndObstacle(player) > 40f)
                destroyGameObject();
        }
    }

    private void moveWheels(float velocity)
    {
        for(int i =0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).Rotate(Vector3.right * Time.deltaTime * velocity*50, Space.Self);
        }
    }

    private float getDistanceBetweenPlayerAndObstacle(GameObject player)
    {
        return player.transform.position.z - transform.position.z;
    }

    public void destroyGameObject()
    {
        if (path != null) path.pathUpdated -= OnPathChanged;
        Destroy(this.gameObject);
    }
}
