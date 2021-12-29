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
    private TextMesh titlePanel;

    [SerializeField]
    private GameObject objectiveText;

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

    private string[] I18NDetails = new string[2];
    [SerializeField]
    private Button firstButton;
    public void ExistenceCheck() {
        if(!GlobalVariables.Instance.IsWorldMenuGameState())
                Destroy(this.gameObject);
    }

    private void OnEnable() {
        GlobalVariables.Instance.SetFocusUiElement(firstButton.gameObject);
        PrepareLevelDetailsPanelFromLevelSettings(GlobalVariables.Instance.actualLevelSettings);
    }
    
    public void GoToLevel(){
        GlobalVariables.Instance.UpdateGamemodeFromLvlSettings();
        GlobalVariables.Instance.playerTargetedByCamera = true;
        GlobalVariables.Instance.actualPanelInteractionType = PanelInteractionType.NO_INTERACTION;
    }

    public void PrepareLevelDetailsPanelFromLevelSettings(LevelSettings lvl){
        //TODO: Load level details into panel
        for(int i = 0; i < lvl.mutators.Length; i++){
            Mutators[i].sprite = MutatorRepresentation[(int)lvl.mutators[i]];
            Mutators[i].gameObject.SetActive(Mutators[i].sprite != null);
        }
        var textObjective = objectiveText.GetComponent<TextMesh>();
        if(textObjective != null) {
            textObjective.text = Constants.OBJECTIVE_LITERAL + (int)lvl.objective;
            var textI18NTextMesh = objectiveText.GetComponent<I18NTextMesh>();
            I18NDetails[0] = ""+lvl.objectiveTarget;
            I18NDetails[1] = lvl.objectiveEspecification;
            if(textI18NTextMesh != null) textI18NTextMesh._updateParams(I18NDetails);
        }
        if(PrizeText != null)
            PrizeText.text = lvl.prize == LevelSettings.PrizeLevel.COINS ? " X " + lvl.prizeDetail : "";
        if(lvl.prize == LevelSettings.PrizeLevel.COINS)
            PrizeMesh.sharedMesh = PrizeMeshes[0];
        else if(lvl.prize == LevelSettings.PrizeLevel.SCROLL)
            PrizeMesh.sharedMesh = PrizeMeshes[1];
        else 
            PrizeMesh.sharedMesh = null;
    }
}
