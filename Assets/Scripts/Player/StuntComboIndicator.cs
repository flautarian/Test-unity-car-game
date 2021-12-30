using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StuntComboIndicator : MonoBehaviour
{

    [SerializeField]
    private Mesh[] comboLevels;

    [SerializeField]
    private MeshFilter comboNumber;

    [SerializeField]
    private BarController barController;

    private Animator animator;
    private int stuntTemp = -1;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void AddComboLevel(){
        if(GlobalVariables.Instance.stuntCombo == 0)
            animator.SetTrigger("Enable");
        else
            animator.SetTrigger("Update");
        GlobalVariables.Instance.stuntCombo++;
        comboNumber.sharedMesh = comboLevels[GlobalVariables.Instance.stuntCombo];
        stuntTemp = 100;
    }

    public void ResetComboIndicator(){
        if(GlobalVariables.Instance.stuntCombo != 0){
            GlobalVariables.Instance.stuntCombo= 0;
            animator.SetTrigger("Disable");
        }
    }

    private void Update() {
        barController.UpdateValue(stuntTemp);
        //if(stuntTemp > 0) stuntTemp--;
        //else 
        if(stuntTemp == 0){
            stuntTemp = -1;
            animator.SetTrigger("Disable");
        }
    }
}