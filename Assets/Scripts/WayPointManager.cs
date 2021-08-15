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

    internal void addPreviousWayPoint(List<WayPoint> previuosWayPoints)
    {
        // Afegim connexio del final de la via normal d'aquest minimapa amb el principi de la via normal del seguent minimapa
        for (int i = 0; i < firstWayPoint.Count; i++)
        {
            firstWayPoint[i].previousWayPoint.Add(previuosWayPoints[i].transform);
        }
    }

    internal void addToNextWayPoint(List<WayPoint> newFirstWayPoints)
    {
        // Afegim connexio del principi de la via normal d'aquest minimapa amb el final de la via normal del minimapa anterior
        for (int i = 0; i < lastWayPoint.Count; i++)
        {
            lastWayPoint[i].nextWayPoint.Add(newFirstWayPoints[i].transform);
        }
    }

    internal void addNextReversalWayPoint(List<WayPoint> lastWayReversalPoint)
    {
        // Afegim connexio del principi de la via contraria del minimapa amb el waypoint del final del seguent minimapa
        for (int i = 0; i < firstWayReversalPoint.Count; i++)
        {
            firstWayReversalPoint[i].previousWayPoint.Add(lastWayReversalPoint[i].transform);
        }
    }

    internal void addPreviousReversalWayPoint(List<WayPoint> firstWayReversalPoint)
    {
        // Afegim connexio del final de la via contraria del minimapa amb el principi la via contraria del minimap anterior
        for (int i = 0; i < lastWayReversalPoint.Count; i++)
        {
            lastWayReversalPoint[i].nextWayPoint.Add(firstWayReversalPoint[i].transform);
        }
    }
}
