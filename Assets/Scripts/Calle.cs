using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calle : MonoBehaviour
{
    // Start is called before the first frame update

    public PathCreation.PathCreator path;
    public PathCreation.PathCreator counterPath;

    private int pathSegmentsNumber;
    private int counterPathSegmentsNumber;
    void Start()
    {
        this.pathSegmentsNumber = path.bezierPath.NumSegments;
        this.counterPathSegmentsNumber = counterPath.bezierPath.NumSegments;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getPathSegmentsNumber()
    {
        return this.pathSegmentsNumber;
    }

    public int getCounterPathSegmentsNumber()
    {
        return this.counterPathSegmentsNumber;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
            other.GetComponent<Player>().EndingStreet(this.gameObject);
    }
}
