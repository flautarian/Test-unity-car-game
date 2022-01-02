using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Honeti;

public class PanelLibraryEquipCanvasController : MonoBehaviour
{
    [SerializeField]
    private Button firstButton;

    [SerializeField]
    private MeshFilter[] scrollMeshFilters;

    [SerializeField]
    private TextMesh[] scrollNames;

    [SerializeField]
    private MeshFilter scrollMeshFilterToEquip;

    [SerializeField]
    private Mesh[] scrollMeshes;

    private int IndexScrollToEquip;

    [SerializeField]
    private StuntsIndicator stuntsIndicator;

    [SerializeField]
    private PanelsCanvasController panelsCanvasController;

    public void ExistenceCheck() {
        if(!GlobalVariables.Instance.IsWorldMenuGameState())
                Destroy(this.gameObject);
    }

    private void OnEnable() {
        if(GlobalVariables.Instance != null){
            var eventSystem = EventSystem.current;
            if(firstButton != null) eventSystem.SetSelectedGameObject( firstButton.gameObject, new BaseEventData(eventSystem));
            LoadPanelInfo();
        }
    }

    private void Update() {
        if(Input.GetButtonDown(Constants.BACK)){
            var anim = panelsCanvasController.GetComponent<Animator>();
            if(anim != null)
                anim.SetTrigger(Constants.ANIMATION_TRIGGER_LIBRARY_INTERACTION);
        }
    }

    private void LoadPanelInfo(){
        Scroll scrollToEquip = GlobalVariables.Instance.GetScroll(IndexScrollToEquip);
        scrollMeshFilterToEquip.sharedMesh = scrollMeshes[scrollToEquip.stuntType];

        Scroll[] equippedScrolls = GlobalVariables.Instance.GetPlayerEquippedScrolls();
        for(int i =0; i < equippedScrolls.Length; i++){
            if(equippedScrolls[i] != null){
                scrollMeshFilters[i].sharedMesh = scrollMeshes[equippedScrolls[i].stuntType];
                scrollNames[i].text = equippedScrolls[i].stuntAssosiated;
            }
            else {
                scrollMeshFilters[i].sharedMesh = scrollMeshes[3];
                scrollNames[i].text = "[]";
            }
        }
    }

    public void setScrollToEquip(int scrollIndex){
        if(GlobalVariables.Instance.IsScrollEnabled(scrollIndex)){
            IndexScrollToEquip = scrollIndex;
            var anim = panelsCanvasController.GetComponent<Animator>();
            if(anim != null)
                anim.SetTrigger(Constants.ANIMATION_TRIGGER_LIBRARY_INTERACTION);
        }
        else{
            GlobalVariables.Instance.GetAndPlayChunk("UI_Ko", 1.0f);
        }
    }

    public void UpdateScrollInEquipment(int index){
        stuntsIndicator.UpdateStuntEquippedListAndReload(index, IndexScrollToEquip);
        GlobalVariables.Instance.SaveGame();
    }
}
