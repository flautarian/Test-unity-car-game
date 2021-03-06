using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SelectButtonController : MonoBehaviour, ISelectHandler
{
    public Animator animator;
    internal Button button;
    private void OnEnable() {
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

    public void OnSelect(BaseEventData eventData)
    {
        GlobalVariables.Instance.GetAndPlayChunk(Constants.CHUNK_MOVE_UI_BUTTON, ((float) UnityEngine.Random.Range(8f, 13f) / 10f));
    }

    public void OnSubmit(BaseEventData eventData)
    {
        GlobalVariables.Instance.GetAndPlayChunk(Constants.CHUNK_MOVE_UI_BUTTON, 1f);
    }

}
