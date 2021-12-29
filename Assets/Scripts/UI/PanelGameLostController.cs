using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelGameLostController : MonoBehaviour
{
    public Button firstButton;

    public void ExistenceCheck() {
        if(!GlobalVariables.Instance.IsLevelGameState())
                Destroy(this.gameObject);
    }
    private void OnEnable() {
        var eventSystem = EventSystem.current;
        if(firstButton != null) eventSystem.SetSelectedGameObject( firstButton.gameObject, new BaseEventData(eventSystem));
    }
}
