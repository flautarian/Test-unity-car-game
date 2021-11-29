using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class SelectButtonController : MonoBehaviour
{
    public Animator animator;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if(animator != null){
            if(EventSystem.current.currentSelectedGameObject == this.gameObject)
                animator.SetBool(Constants.ANIMATION_BUTTON_CONTROLLER_SELECTED_BOOL, true);
            else animator.SetBool(Constants.ANIMATION_BUTTON_CONTROLLER_SELECTED_BOOL, false);
        }
    }

    public void TriggerButton(){
        animator.SetBool(Constants.ANIMATION_BUTTON_CONTROLLER_TRIGGERED_BOOL, true);
        executeButton();
    }    

    public abstract void executeButton();

}
