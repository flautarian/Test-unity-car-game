using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPathJoin : MonoBehaviour
{
    // Start is called before the first frame update

    public bool activate = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activate) joinAllPaths();
    }

    private void joinAllPaths()
    {
        PathCreation.PathCreator[] list = this.GetComponentsInChildren<PathCreation.PathCreator>(false);
        joinPath(list[0], list[1]);
        joinPath(list[0], list[2]);
        Destroy(list[1].gameObject);
        Destroy(list[2].gameObject);
        activate = false;
    }

    private void joinPath(PathCreation.PathCreator pathA, PathCreation.PathCreator pathB)
    {
        try
        {
            for (int i = 0; i < pathB.bezierPath.NumSegments; i++)
            {
                Vector3 vec = pathB.bezierPath.GetPointsInSegment(i)[0];
                vec.z += pathB.transform.position.z;
                pathA.bezierPath.AddSegmentToEnd(vec);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }
}
