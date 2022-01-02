using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Honeti;

public class MuseumSelectionButton : SelectButtonController, ISelectHandler
{

    [SerializeField]
    internal int RelicIndex;

    [SerializeField]
    internal PanelMuseumCanvasController panelController;

    [SerializeField]
    private Text nameText;
    [SerializeField]
    private I18NText nameI18n;

    private void OnEnable() {
        nameText.text = GlobalVariables.Instance.IsRelicEnabled(RelicIndex) ? 
        "^relic_name_" + RelicIndex :
        "^relic_name_hidden";
    }

    public void OnSelect(BaseEventData eventData)
    {
        panelController.RefreshPanels(RelicIndex);
    }

    public override void executeButton(){
        
    }
}
