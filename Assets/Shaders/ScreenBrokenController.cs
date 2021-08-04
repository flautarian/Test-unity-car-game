using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScreenBrokenController : MonoBehaviour
{
    [SerializeField] private ForwardRendererData rendererData = null;
    [SerializeField] private string featureName = null;
    [Range(0f, 2f)] private float brokenScale = 0, brokeScaleSentinel =0;

    void Update()
    {
        if (brokeScaleSentinel != GlobalVariables.Instance.currentBrokenScreen) brokenScale = GlobalVariables.Instance.currentBrokenScreen;
        brokeScaleSentinel = GlobalVariables.Instance.currentBrokenScreen;
        updateBrokenScale();
        if (brokenScale > 0) brokenScale -= 0.001f;
        else brokenScale = 0;
    }

    private void updateBrokenScale()
    {
        if (TryGetFeature(out var feature))
        {
            var blitFeature = feature as BlitRenderPassFeature;
            var material = blitFeature.Material;
            material.SetFloat("_BrokenScale", brokenScale);
        }
    }

    private void OnDestroy()
    {
        brokenScale = 0;
        updateBrokenScale();
    }

    private bool TryGetFeature(out ScriptableRendererFeature feature)
    {
        feature = rendererData.rendererFeatures.Where((f) => f.name == featureName).FirstOrDefault();
        foreach (ScriptableRendererFeature renderF in rendererData.rendererFeatures)
        {
            Debug.Log(renderF.name);
        }
        return feature != null;
    }
}
