using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calle : MonoBehaviour
{
    // Start is called before the first frame update

    public PathCreation.PathCreator path;
    public PathCreation.PathCreator counterPath;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addSegmentsToCalle(PathCreation.PathCreator pathB)
    {
        joinPath(path, pathB);
        path.TriggerPathUpdate();
    }


    private void joinPath(PathCreation.PathCreator pathA, PathCreation.PathCreator pathB)
    {
        for (int i = 0; i < pathB.bezierPath.NumSegments; i++)
        {
            pathA.bezierPath.AddSegmentToEnd(pathB.bezierPath.GetPointsInSegment(i)[0]);
        }
        //pathA.TriggerPathUpdate();
    }
}
