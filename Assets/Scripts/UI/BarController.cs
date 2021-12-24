using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    public Material barMaterial;
    public MeshFilter meshFilter;
    private int barValue, newBarValue;
    public float progressBorder, fillRate, fillPercentValue;
    void Start()
    {
        barMaterial = GetComponent<MeshRenderer>().material;
        meshFilter = GetComponent<MeshFilter>();
        barValue = GlobalVariables.Instance.totalStuntEC;
        progressBorder = meshFilter.mesh.bounds.size.x / 2f;
        barMaterial.SetFloat(Constants.SHADER_CONTROL_STUNT_BAR_PROGRESS_BORDER, progressBorder);
        fillRate = -progressBorder;
        barMaterial.SetFloat(Constants.SHADER_CONTROL_STUNT_BAR_FILL_RATE, fillRate);
        fillPercentValue = ( 2* progressBorder) / 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if(newBarValue != barValue){
            barValue = newBarValue;
            barMaterial.SetFloat(Constants.SHADER_CONTROL_STUNT_BAR_FILL_RATE, fillRate + (fillPercentValue * barValue));
        }
    }

    public void UpdateValue(int newVal){
        newBarValue = newVal;
    }
}
