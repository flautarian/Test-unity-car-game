using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelCanvasController : MonoBehaviour
{
    public Button firstButton;

    private void OnEnable() {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject( firstButton.gameObject, new BaseEventData(eventSystem));
    }
}
