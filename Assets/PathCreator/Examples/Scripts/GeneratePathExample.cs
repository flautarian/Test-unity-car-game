using UnityEngine;

namespace PathCreation.Examples {
    // Example of creating a path at runtime from a set of points.

    [RequireComponent(typeof(PathCreator))]
    public class GeneratePathExample : MonoBehaviour {

        public bool closedLoop = false;
        public bool activateAddWayPoint = false;
        private int wayPointsDistance = 25;

        public Transform[] waypoints;

        void Start () {
            if (waypoints.Length > 0) {
                // Create a new bezier path from the waypoints.
                BezierPath bezierPath = new BezierPath (waypoints, closedLoop, PathSpace.xyz);
                GetComponent<PathCreator> ().bezierPath = bezierPath;
            }
        }

        private void Update()
        {
            if (activateAddWayPoint)
            {
                Transform t = Instantiate(waypoints[0]);
                t.Translate(Vector3.forward * (wayPointsDistance));
                GetComponent<PathCreator>().bezierPath.AddSegmentToEnd(t.position);
                wayPointsDistance += 25;
                activateAddWayPoint = false;
            }
        }
    }
}