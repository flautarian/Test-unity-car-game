using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    bool m_Started;
    public Vector3 pathOffset;
    public float velocity;
    
    private float dstTravelledToBeReady;
    public LayerMask m_LayerMask;

    public PathCreator path { get; set; }
    public float dstTravelled { get; set; }
    public System.Random rand { get; set; }
    public bool isReadyToInstanceObstacle { get; set; }

    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        dstTravelledToBeReady = 10;
        m_Started = true;
        isReadyToInstanceObstacle = false;
        rand = new System.Random(); ;
    }

    public int MyProdstTravelledperty { get; set; }

    void Update()
    {
        if (velocity > 0)
        {
            dstTravelled += velocity * Time.deltaTime;
            if (path != null)
            {
                Vector3 pos = path.path.GetPointAtDistance(dstTravelled);
                if (pos.z > transform.position.z)
                {
                    pos += pathOffset;
                    transform.position = pos;
                    transform.rotation = path.path.GetRotationAtDistance(dstTravelled);
                    if(dstTravelled > dstTravelledToBeReady) isReadyToInstanceObstacle = true;
                }
                else isReadyToInstanceObstacle = false;
            }
        }
    }

    public void ReSetInstanceTravelledToBeReady()
    {
        dstTravelledToBeReady= dstTravelled + rand.Next(15, 30);
        isReadyToInstanceObstacle = false;
    }

    private void OnDestroy()
    {
        if (path != null) path.pathUpdated -= OnPathChanged;
    }
    private void OnPathChanged()
    {
        if (path != null) dstTravelled = path.path.GetClosestDistanceAlongPath(transform.position);
    }

    public bool spawnIsOverlappingOtherCollitions()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(gameObject.transform.position, transform.localScale, 0, m_LayerMask);
        foreach(Collider2D col in hitColliders)
        {
            if (!col.tag.Equals("Calle")) return true;
        }
        return false;
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = isReadyToInstanceObstacle ? Color.green : Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    public void setPath(PathCreator mainPath)
    {
        if (mainPath != null)
        {
            path = mainPath;
            transform.position = mainPath.path.GetClosestPointOnPath(transform.position);
            path.pathUpdated += OnPathChanged;
        }
    }
}
