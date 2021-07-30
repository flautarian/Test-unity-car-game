using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicLight : MonoBehaviour
{
    private new Light light;
    private float lightIndex;
    void Start()
    {
        light = GetComponent<Light>();
        lightIndex = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if(light != null)
        {
            light.intensity = lightIndex * GlobalVariables.Instance.currentLight;
        }
    }
}
