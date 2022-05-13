using UnityEngine.Rendering.Universal;
using System.Linq;
using UnityEngine;

public class RadialBlurController : MonoBehaviour
{
    [SerializeField] private UniversalRendererData rendererData = null;
    [SerializeField] private string featureName = null;
    [Range(0f, 2f)] private float currentIntensity = 0;

    void Update()
    {
        //if (Input.GetAxis("Vertical") != 0f) currentIntensity += 0.05f;
        //else currentIntensity -= 0.1f;
        currentIntensity = GlobalVariables.Instance.currentRadialBlur;
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
        return feature != null;
    }
}
