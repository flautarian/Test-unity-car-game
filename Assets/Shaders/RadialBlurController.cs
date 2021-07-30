using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using System.Linq;
using UnityEngine;

public class RadialBlurController : MonoBehaviour
{
    [SerializeField] private ForwardRendererData rendererData = null;
    [SerializeField] private string featureName = null;

    private bool transitioning;

    [Header("Attributes")]

    [Range(0f, 2f)]
    private float currentIntensity = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0f) currentIntensity += 0.05f;
        else currentIntensity -= 0.1f;
        currentIntensity = Mathf.Clamp(currentIntensity, 0, 2);
        if (TryGetFeature(out var feature))
        {
            var blitFeature = feature as BlitRenderPassFeature;
            var material = blitFeature.Material;
            material.SetFloat("_EffectAmount", currentIntensity);
        }
    }
    private void OnDestroy()
    {
        currentIntensity = 0;
    }

    private bool TryGetFeature(out ScriptableRendererFeature feature)
    {
        feature = rendererData.rendererFeatures.Where((f) => f.name == featureName).FirstOrDefault();
        foreach(ScriptableRendererFeature renderF in rendererData.rendererFeatures){
            Debug.Log(renderF.name);
        }
        return feature != null;
    }
}
