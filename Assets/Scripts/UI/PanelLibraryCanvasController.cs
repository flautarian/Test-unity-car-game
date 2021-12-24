using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Honeti;

public class PanelLibraryCanvasController : MonoBehaviour
{
    [SerializeField]
    private Button firstButton;

    [SerializeField]
    private Transform[] combination;

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

    private void OnEnable() {
        var eventSystem = EventSystem.current;
        if(firstButton != null) eventSystem.SetSelectedGameObject( firstButton.gameObject, new BaseEventData(eventSystem));
        RefreshPanels(0);
    }

    public void RefreshPanels(int StuntIndex){
        if(GlobalVariables.Instance.IsScrollEnabled(StuntIndex)){
            Scroll scrollToShow = GlobalVariables.Instance.GetScroll(StuntIndex);
            for(int i = 0; i < scrollToShow.combination.Length; i++){
                var rot = combination[i].rotation;
                switch(scrollToShow.combination[i]){
                    case 0:
                        combination[i].gameObject.SetActive(true);
                        rot.eulerAngles = up;
                    break;
                    case 1:
                        combination[i].gameObject.SetActive(true);
                        rot.eulerAngles = down;
                    break;
                    case 2:
                        combination[i].gameObject.SetActive(true);
                        rot.eulerAngles = left;
                    break;
                    case 3:
                        combination[i].gameObject.SetActive(true);
                        rot.eulerAngles = right;
                    break;
                    default:
                        combination[i].gameObject.SetActive(false);
                    break;
                }
                combination[i].rotation = rot;
            }

            scrollMeshFilter.sharedMesh = scrollMeshes[scrollToShow.stuntType];

            description.text = scrollToShow.description;
        }
        else{
            // hidden scroll
            scrollMeshFilter.sharedMesh = scrollMeshes[3];
            for(int i = 0; i < 5; i++){
                combination[i].gameObject.SetActive(false);
            }
            description.text = "^stunt_description_hidden";
        }
        descriptionI18n.updateTranslation(true);
    }
}
