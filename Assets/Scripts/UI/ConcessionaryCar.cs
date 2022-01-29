using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcessionaryCar : MonoBehaviour
{
    
    [SerializeField]
    internal Mesh CCar;
    
    [SerializeField]
    internal int keyCode = -1;
    [SerializeField]
    public float turnRadius = 10f;
    [SerializeField]
	public float torque = 25f;
    [SerializeField]
	public float maxRpm = 1100f;
    [SerializeField]
	public float brakeTorque = 100f;
    [SerializeField]
	public float stuntHability = 1.0f;
    [SerializeField]
	public float drag = 0.1f;
    [SerializeField]
    internal int price;

    [SerializeField]
    internal string carName;
}
