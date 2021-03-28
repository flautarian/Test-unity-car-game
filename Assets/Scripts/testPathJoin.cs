using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPathJoin : MonoBehaviour
{
    // Start is called before the first frame update

    public bool activate = false;

    void Start()
    {
        joinAllPaths();
    }

    // Update is called once per frame
    void Update()
    {
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
        for (int i = 0; i < pathB.bezierPath.NumSegments; i++)
        {
            pathA.bezierPath.AddSegmentToEnd(pathB.bezierPath.GetPointsInSegment(i)[0]);
            pathA.TriggerPathUpdate();
        }
        
    }
}
