using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInfo : MonoBehaviour
{
    [SerializeField]
    internal List<PlayerDestructablePart> parts;
    [SerializeField]
    internal List<Transform> wheels;
    [SerializeField]
    internal float idealRPM = 500.0f;
    [SerializeField]
	internal float maxRPM = 1000f;
    [SerializeField]
	internal float turnRadius = 6f;
    [SerializeField]
	internal float torque = 25f;
    [SerializeField]
	internal float brakeTorque = 100f;
    [SerializeField]
	internal float stuntHability = 1.0f;
}
