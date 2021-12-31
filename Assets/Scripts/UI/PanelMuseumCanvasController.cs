using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Honeti;

public class PanelMuseumCanvasController : MonoBehaviour
{
    [SerializeField]
    private Button firstButton;

    [SerializeField]
    private MeshFilter scrollMeshFilter;

    [SerializeField]
    private TextMesh description;
    [SerializeField]
    private I18NTextMesh descriptionI18n;

    private Vector3 up = new Vector3(90f,0f,0f);
    private Vector3 down = new Vector3(-90f,0f,180f);
    private Vector3 left = new Vector3(0f,-90f,-90f);
    private Vector3 right = new Vector3(0f,90f,90f);

    [SerializeField]
    private Mesh[] scrollMeshes;

    [SerializeField]
    private Mesh missingScroll;
    public void ExistenceCheck() {
        if(!GlobalVariables.Instance.IsWorldMenuGameState())
                Destroy(this.gameObject);
    }

    private void OnEnable() {
        var eventSystem = EventSystem.current;
        if(firstButton != null) eventSystem.SetSelectedGameObject( firstButton.gameObject, new BaseEventData(eventSystem));
        RefreshPanels(0);
    }

    public void RefreshPanels(int Index){
        if(GlobalVariables.Instance.IsRelicEnabled(Index)){
            scrollMeshFilter.sharedMesh = scrollMeshes[Index];
            description.text = "^relic_description_" + Index;
        }
        else{
            // hidden scroll
            scrollMeshFilter.sharedMesh = missingScroll;
            description.text = "^relic_description_hidden";
        }
        descriptionI18n.updateTranslation(true);
    }
}
