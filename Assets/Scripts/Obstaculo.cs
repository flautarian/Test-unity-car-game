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
    private bool automaticDriving = true;
    public PathCreator path;
    private GameObject player;
    public Vector3 localPosition;
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
            if (automaticDriving)
            {
                if (path != null){
                    transform.position = path.path.GetPointAtDistance(dstTravelled);
                    Quaternion rot = path.path.GetRotationAtDistance(dstTravelled);
                    transform.rotation = rot;
                }
            }
            else GetComponent<Transform>().Translate(Vector3.forward * Time.deltaTime * velocity);
            
            if (player != null && getDistanceBetweenPlayerAndObstacle(player) > 40f)
                destroyGameObject();
        }
    }

    void LateUpdate()
    {
        if (!GetComponent<Animation>().isPlaying)
            return;
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

    private float getDistanceBetweenPlayerAndObstacle(GameObject player)
    {
        return player.transform.position.z - transform.position.z;
    }

    public void destroyGameObject()
    {
        if (path != null) path.pathUpdated -= OnPathChanged;
        Destroy(this.gameObject);
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
}
