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
    private Light decorationLight;
    void Start()
    {
        lightStatus = lightInitialStatus;
        decorationLight = GetComponent<Light>();
        secondsSentinel = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (decorationLight != null)
        {
            if (blinks)
            {
                secondsSentinel += Time.deltaTime;
                if (secondsSentinel % 60 > secondsBetweenBlinks)
                {
                    lightStatus = !lightStatus;
                    secondsSentinel = 0;
                }
            }
            decorationLight.enabled = lightStatus;
        }
    }
}
