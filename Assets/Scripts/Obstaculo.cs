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

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Transform>().Translate(Vector2.up * Time.deltaTime * velocity);

        if(velocity > 0)
        {
            dstTravelled += velocity * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(dstTravelled);
            Quaternion rot = pathCreator.path.GetRotationAtDistance(dstTravelled);
            rot.x += 90;
           rot.y += 90;

            transform.rotation = rot;
            if (System.Object.Equals(player , null)) player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && getDistanceBetweenPlayerAndObstacle(player) > 40f)
                Destroy(this.gameObject);
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
