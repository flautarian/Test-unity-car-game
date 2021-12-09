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

    private void OnEnable() {
        var eventSystem = EventSystem.current;
        if(firstButton != null) eventSystem.SetSelectedGameObject( firstButton.gameObject, new BaseEventData(eventSystem));
        LoadPanelInfo();
    }

    private void LoadPanelInfo(){
        Scroll scrollToEquip = GlobalVariables.Instance.GetScroll(IndexScrollToEquip);
        scrollMeshFilterToEquip.sharedMesh = scrollMeshes[scrollToEquip.stuntType];

        Scroll[] equippedScrolls = GlobalVariables.Instance.GetPlayerEquippedScrolls();
        for(int i =0; i < equippedScrolls.Length; i++){
            if(equippedScrolls[i] != null){
                scrollMeshFilters[i].sharedMesh = scrollMeshes[equippedScrolls[i].stuntType];
            }
            else scrollMeshFilters[i].sharedMesh = scrollMeshes[3];
        }
    }

    public void setScrollToEquip(int scrollIndex){
        IndexScrollToEquip = scrollIndex;
    }

    public void UpdateScrollInEquipment(int index){
        stuntsIndicator.UpdateStuntEquippedListAndReload(index, IndexScrollToEquip);
        GlobalVariables.Instance.SaveGame();
    }
}
