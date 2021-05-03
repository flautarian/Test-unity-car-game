using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calle : MonoBehaviour
{
    // Start is called before the first frame update

    public PathCreation.PathCreator path;

    private int pathSegmentsNumber;
    void Start()
    {
        this.pathSegmentsNumber = path.bezierPath.NumSegments;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getPathSegmentsNumber()
    {
        return this.pathSegmentsNumber;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
            other.GetComponent<Player>().EndingStreet(this.gameObject);
    }
}
