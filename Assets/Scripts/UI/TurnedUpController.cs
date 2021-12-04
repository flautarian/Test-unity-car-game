using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TurnedUpController : MonoBehaviour
{
    public Transform leftArrow, rightArrow;
    public int progress = 0;
    private bool side = false;
    public float HorizontalAxis = 0;
    private Vector3 nonScaledArrowVector = new Vector3(50f, 50f, 50f);
    private Vector3 scaledArrowVector = new Vector3(60f, 60f, 60f);
    public Material barMaterial;
    public MeshFilter meshFilter;
    public Text panelText;
    private Animator animator;
    private float progressBorder, fillRate, fillPercentValue;

    private void OnEnable() {
        progress = 0;
        if(animator != null) animator.SetTrigger(Constants.ANIMATION_TRIGGER_TURNED_PANEL_BUTTON);
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        progressBorder = meshFilter.mesh.bounds.size.x / 2f;
        barMaterial.SetFloat(Constants.SHADER_CONTROL_STUNT_BAR_PROGRESS_BORDER, progressBorder);
        fillRate = -progressBorder;
        barMaterial.SetFloat(Constants.SHADER_CONTROL_STUNT_BAR_FILL_RATE, fillRate);
        fillPercentValue = ( 2* progressBorder) / 100f;
    }

    private void FixedUpdate() {
        if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT)) && !side){
            //lft
            progress += 15;
            side = true;
        }
        else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT)) && side){
            //rgt
            progress += 15;
            side = false;
        }
        if(progress > 1 && progress < 99) progress -= 1;

        if(progress > 99)
            animator.SetTrigger(Constants.ANIMATION_TRIGGER_DISABLE_TURNED_PANEL_BUTTON);
        
        barMaterial.SetFloat(Constants.SHADER_CONTROL_STUNT_BAR_FILL_RATE, fillRate + (fillPercentValue * progress));

        leftArrow.transform.localScale = Vector3.Lerp (leftArrow.transform.localScale, !side ? scaledArrowVector : nonScaledArrowVector, 10 * Time.deltaTime);
        rightArrow.transform.localScale = Vector3.Lerp (rightArrow.transform.localScale, side ? scaledArrowVector : nonScaledArrowVector, 10 * Time.deltaTime);
        
    }

    public void DeactivateTurnUpFlag(){
        GlobalVariables.Instance.turnedCar = false;
    }
}
