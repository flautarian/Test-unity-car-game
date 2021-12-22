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

    private int actualComboMultiplier = 0;
    private int stuntTemp = -1;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void AddComboLevel(){
        if(actualComboMultiplier == 0)
            animator.SetTrigger("Enable");
        else
            animator.SetTrigger("Update");
        actualComboMultiplier++;
        comboNumber.sharedMesh = comboLevels[actualComboMultiplier];
        stuntTemp = 100;
    }

    public void ResetComboIndicator(){
        if(actualComboMultiplier != 0){
            actualComboMultiplier= 0;
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