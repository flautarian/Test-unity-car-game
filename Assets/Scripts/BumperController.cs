using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperController : MonoBehaviour
{
    public MovableObstacle car;
    private GameObject carDetected = null;
    private MovableObstacle carDetectedClass = null;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag.Equals(Constants.POOL_STREET_OBSTACLE) && carDetected == null)
        {
            carDetected = c.gameObject;
            carDetectedClass = carDetected.GetComponent<MovableObstacle>();
            if (carDetectedClass != null) carDetectedClass.frontalCarBumperDetected(car.vel);
        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (carDetectedClass != null) carDetectedClass.frontalCarBumperDetected(car.vel);
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.Equals(carDetected))
        {
            carDetectedClass.frontalCarBumperHidden();
            carDetected = null;
            carDetectedClass = null;
        }
    }
}
