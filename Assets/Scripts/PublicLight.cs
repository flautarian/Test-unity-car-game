using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicLight : MonoBehaviour
{
    private Light light;
    private float lightIndex;
    private float intensity;

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
            intensity = lightIndex * GlobalVariables.Instance.currentLight;
            light.intensity = intensity;
            light.enabled = intensity > 0;
        }
    }
}
