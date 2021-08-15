using System;
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
    internal bool isOccuped;

    [SerializeField]
    internal GameObject vehicleLockingWaypoint;

    [SerializeField]
    public int order;

    internal bool lockWaypoint( GameObject locker)
    {
        isOccuped = true;
        vehicleLockingWaypoint = locker;
        return true;
    }

    internal void unlockWaypoint()
    {
        isOccuped = false;
    }

    internal bool tryLockIncorporation(GameObject lockerCandidate)
    {
        if (isAPreludeOfIncorporation())
        {
            GameObject nextIncorpCand = nextWayPoint[0].gameObject;
            WayPoint wpCand = nextIncorpCand.GetComponent<WayPoint>();
            if (!wpCand.isOccuped) return wpCand.lockWaypoint(lockerCandidate);
            else return false;
        }
        return true;
    }

    internal bool isAPreludeOfIncorporation()
    {
        if (nextWayPoint.Count != 1) return false;
        else
        {
            if (nextWayPoint[0] == null || nextWayPoint[0].gameObject == null) return false;
            GameObject nextIncorpCand = nextWayPoint[0].gameObject;
            WayPoint wpCand = nextIncorpCand.GetComponent<WayPoint>();
            return wpCand.previousWayPoint.Count > 1;
        }
    }

    internal bool itWasAnIncorporation()
    {
        if (nextWayPoint.Count > 1) return false;
        else
        {
            GameObject previousIncorpCand = previousWayPoint[0].gameObject;
            WayPoint wpCand = previousIncorpCand.GetComponent<WayPoint>();
            return wpCand.previousWayPoint.Count > 1;
        }
    }

    internal bool isAnIncorporation()
    {
        return previousWayPoint.Count > 1;
    }
}
