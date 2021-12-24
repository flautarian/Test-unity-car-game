using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LibrarySelectionButton : SelectButtonController, ISelectHandler
{

    [SerializeField]
    internal int StuntIndex;

    [SerializeField]
    internal PanelLibraryCanvasController panelController;

    public void OnSelect(BaseEventData eventData)
    {
        panelController.RefreshPanels(StuntIndex);
    }

    public override void executeButton(){
        
    }
}
