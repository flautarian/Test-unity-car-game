using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    [SerializeField]
    public List<Transform> nextWayPoint;

    [SerializeField]
    public List<Transform> previousWayPoint;

    [SerializeField]
    public bool isReverse;

    [SerializeField]
    public int order;

}
