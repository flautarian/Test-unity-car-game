using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationLight : MonoBehaviour
{
    public bool blinks;
    public float secondsBetweenBlinks;
    private float secondsSentinel;
    private bool lightStatus;
    public bool lightInitialStatus;
    private Light light;
    void Start()
    {
        lightStatus = lightInitialStatus;
        light = GetComponent<Light>();
        secondsSentinel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(light != null)
        {
            if (blinks)
            {
                secondsSentinel += Time.deltaTime;
                if(secondsSentinel % 60 > secondsBetweenBlinks)
                {
                    lightStatus = !lightStatus;
                    secondsSentinel = 0;
                }
            }
            light.enabled = lightStatus;
        }
    }
}
