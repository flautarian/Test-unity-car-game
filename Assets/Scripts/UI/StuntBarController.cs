using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntBarController : MonoBehaviour
{
    public Material barMaterial;
    public MeshFilter meshFilter;
    public int barValue;
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
        if(GlobalVariables.Instance.totalStuntEC != barValue){
            barValue = GlobalVariables.Instance.totalStuntEC;
            barMaterial.SetFloat(Constants.SHADER_CONTROL_STUNT_BAR_FILL_RATE, fillRate + (fillPercentValue * barValue));
        }
    }
}
