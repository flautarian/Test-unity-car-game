using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WayPoint : MonoBehaviour
{

    [SerializeField]
    public List<WayPoint> nextWayPoint;

    [SerializeField]
    public List<WayPoint> previousWayPoint;

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
            if (!nextWayPoint[0].isOccuped) return nextWayPoint[0].lockWaypoint(lockerCandidate);
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
            return nextWayPoint[0].previousWayPoint.Count > 1;
        }
    }

    internal bool itWasAnIncorporation()
    {
        if (nextWayPoint.Count > 1) return false;
        else
        {
            return nextWayPoint[0].previousWayPoint.Count > 1;
        }
    }

    internal bool isAnIncorporation()
    {
        return previousWayPoint.Count > 1;
    }

    internal WayPoint GetNextWayPoint(int streetN){
        var tmpList = GetNextWayPointsWithNumber(streetN);
        if(tmpList.Count == 0) return null;
        var chosen = Random.Range(0, tmpList.Count);
        Debug.Log("chosen path: "+ chosen);
        return tmpList[chosen];
    }

    internal List<WayPoint> GetNextWayPointsWithNumber(int n){
        return nextWayPoint.Where(x=>x.order == n).ToList();
    }



}
