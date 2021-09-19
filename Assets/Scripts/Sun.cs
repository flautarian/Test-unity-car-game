using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float sunLightIndex;
    public Light sunLight;

    private void Start()
    {
        sunLight = GetComponent<Light>();
    }

    private void Update()
    {
        GlobalVariables.Instance.currentLight = sunLightIndex;
    }

}
