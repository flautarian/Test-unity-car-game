using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;
using UnityEngine.EventSystems;

public class LvlDetailsPanelController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private TextMesh titleText;

    [SerializeField]
    private I18NTextMesh titleI18N;

    [SerializeField]
    private I18NText objectiveI18N;

    [SerializeField]
    private Text objectiveText;

    [SerializeField]
    private MeshFilter PrizeMesh;

    [SerializeField]
    private Mesh[] PrizeMeshes;

    [SerializeField]
    private TextMesh PrizeText;

    [SerializeField]
    private Image[] Mutators;
    
    [SerializeField]
    private Sprite[] MutatorRepresentation;

    private int lvlIndex = -1;

    private string[] I18NDetails = new string[2];
    [SerializeField]
    private Button firstButton;
    public void ExistenceCheck() {
        if(!GlobalVariables.Instance.IsWorldMenuGameState())
                Destroy(this.gameObject);
    }

    private void OnEnable() {
        GlobalVariables.Instance.SetFocusUiElement(firstButton.gameObject);
    }
    
    public void GoToLevel(){
        GlobalVariables.Instance.UpdateGamemodeFromLvlSettings();
        GlobalVariables.Instance.playerTargetedByCamera = true;
        GlobalVariables.Instance.actualPanelInteractionType = PanelInteractionType.NO_INTERACTION;
    }

    private void LateUpdate() {
        if(lvlIndex == -1 || lvlIndex != GlobalVariables.Instance.actualLevelSettings.lvlIndex){
            lvlIndex = GlobalVariables.Instance.actualLevelSettings.lvlIndex;
            PrepareLevelDetailsPanelFromLevelSettings(GlobalVariables.Instance.actualLevelSettings);
        }
    }

    public void PrepareLevelDetailsPanelFromLevelSettings(LvlSettings lvl){
        Debug.Log("Updating lvl details info: " + (int)lvl.objective);
        for(int i = 0; i < lvl.mutators.Length; i++){
            Mutators[i].sprite = MutatorRepresentation[(int)lvl.mutators[i]];
            Mutators[i].gameObject.SetActive(Mutators[i].sprite != null);
        }
        titleText.text = lvl.levelName;
        titleI18N.updateTranslation(true);
        
        if(objectiveText != null) {
            objectiveText.text = Constants.OBJECTIVE_LITERAL + (int)lvl.objective;
            I18NDetails[0] = ""+lvl.objectiveTarget;
            I18NDetails[1] = lvl.objectiveEspecification;
            objectiveI18N._updateParams(I18NDetails);
            objectiveI18N.updateTranslation(true);
        }
        if(PrizeText != null)
            PrizeText.text = lvl.prize == LvlSettings.PrizeLevel.COINS ? " X " + lvl.prizeDetail : "";
        if(lvl.prize == LvlSettings.PrizeLevel.COINS)
            PrizeMesh.sharedMesh = PrizeMeshes[0];
        else if(lvl.prize == LvlSettings.PrizeLevel.SCROLL)
            PrizeMesh.sharedMesh = PrizeMeshes[1];
        else 
            PrizeMesh.sharedMesh = null;
    }
}
