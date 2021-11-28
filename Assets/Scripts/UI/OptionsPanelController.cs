using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicPanels;

public class OptionsPanelController : MonoBehaviour
{
    private Panel panel;
    private float HorizontalButtonPressed;
    private int ActualTab =0, TabVariation =0;
    private bool blockedKey = false, blockedByOption = false;

    [SerializeField]
    private GameObject[] MainTabButtons;
    void Update()
    {
        if(panel == null) panel = PanelNotificationCenter.GetPanel(0);
        else{
            HorizontalButtonPressed = Input.GetAxisRaw(Constants.AXIS_HORIZONTAL);
            if(!blockedKey && !blockedByOption){
                if(HorizontalButtonPressed > 0 && ActualTab  < panel.NumberOfTabs - 1) TabVariation = 1;                
                else if(HorizontalButtonPressed < 0 && ActualTab  > 0) TabVariation = -1;
                if(TabVariation != 0){
                    ActualTab += TabVariation;
                    GlobalVariables.Instance.SetFocusUiElement(MainTabButtons[ActualTab]);
                    panel.ActiveTab = ActualTab;
                    TabVariation =0;
                }
            }
            if(HorizontalButtonPressed != 0) blockedKey = true;
            else blockedKey = false;
        }
    }
    
    public void LockTabTransition(){
        blockedByOption = true;
    }

    public void UnlockTabTransition(){
        blockedByOption = false;
    }

    public void closeOptionsPanel(){

    }
}
