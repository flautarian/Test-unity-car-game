using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float sunLight;
    public Light light;

    private void Start()
    {
        light = GetComponent<Light>(); 
    }

    private void Update()
    {
        //Debug.Log("" + light.color.r);
        GlobalVariables.Instance.currentLight = sunLight;
    }

}
