using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    // Start is called before the first frame update

    public float velocity;
    public float penalty;
    public PathCreator pathCreator;
    private GameObject player;
    private float dstTravelled = 0;
    void Start()
    {
    }

    public void updatePathChangeFunction()
    {
        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    private void OnPathChanged()
    {
        //if(pathCreator != null) dstTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Transform>().Translate(Vector2.up * Time.deltaTime * velocity);

        if(velocity > 0)
        {
            dstTravelled += velocity * Time.deltaTime;
            moveWheels(velocity);
            if(pathCreator != null){
                transform.position = pathCreator.path.GetPointAtDistance(dstTravelled);
                Quaternion rot = pathCreator.path.GetRotationAtDistance(dstTravelled);
                transform.rotation = rot;
            }
            else GetComponent<Transform>().Translate(Vector3.forward * Time.deltaTime * velocity);
            if (System.Object.Equals(player , null)) player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && getDistanceBetweenPlayerAndObstacle(player) > 40f)
                Destroy(this.gameObject);
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
        return Math.Abs(player.transform.position.y - transform.position.y);
    }

    public void destroyGameObject()
    {
        Destroy(this.gameObject);
    }
}
