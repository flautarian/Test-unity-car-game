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
        ManageLights();
    }

    public void ManageLights(){
        if (publicLight != null){
            intensity = lightIndex * GlobalVariables.Instance.currentLight;
            publicLight.intensity = intensity;
            publicLight.enabled = intensity > 0;
            if(GlobalVariables.Instance.currentLight > 0.1)
                Destroy(this.gameObject);
        }
    }
}
