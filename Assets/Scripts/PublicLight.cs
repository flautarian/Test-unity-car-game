using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicLight : MonoBehaviour
{
    private Light publicLight;
    private float lightIndex;
    private float intensity;

    void Start()
    {
        publicLight = GetComponent<Light>();
        lightIndex = publicLight.intensity;
        if(GlobalVariables.Instance.currentLight < 10000)
            Destroy(this.gameObject);
    }

    // Update is called once per frame

    private void Update()
    {
        if (publicLight != null)
        {
            intensity = lightIndex * GlobalVariables.Instance.currentLight;
            publicLight.intensity = intensity;
            publicLight.enabled = intensity > 0;
        }
    }
}
