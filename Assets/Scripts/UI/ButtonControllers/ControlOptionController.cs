using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ControlOptionController : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    [SerializeField]
    private OptionsPanelController OptionsPanelController;

    public void OnSelect(BaseEventData eventData)
    {
        OptionsPanelController.LockTabTransition();
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        OptionsPanelController.UnlockTabTransition();
    }
 
}
