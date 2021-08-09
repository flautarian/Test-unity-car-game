using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{

    public List<WayPoint> firstWayPoint;
    public List<WayPoint> lastWayPoint;

    public List<WayPoint> firstWayReversalPoint;
    public List<WayPoint> lastWayReversalPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void addToNextWayPoint(Transform nextWayPoint, int i) =>
        // Afegim connexio del final de la via normal d'aquest minimapa amb el principi de la via normal del seguent minimapa
        lastWayPoint[i].nextWayPoint.Add(nextWayPoint);

    internal void addPreviousWayPoint(Transform previousWayPoint, int i) =>
        // Afegim connexio del principi de la via normal d'aquest minimapa amb el final de la via normal del minimapa anterior
        firstWayPoint[i].previousWayPoint.Add(previousWayPoint);

    internal void addPreviousReversalWayPoint(Transform previousReversalWayPoint, int i) =>
        // Afegim connexio del final de la via contraria del minimapa amb el principi la via contraria del minimap anterior
        lastWayReversalPoint[i].nextWayPoint.Add(previousReversalWayPoint);

    internal void addNextReversalWayPoint(Transform nextReversalWayPoint, int i) =>
        // Afegim connexio del principi de la via contraria del minimapa amb el waypoint del final del seguent minimapa
        firstWayReversalPoint[i].previousWayPoint.Add(nextReversalWayPoint);
}
