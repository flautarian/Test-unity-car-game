using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Honeti;

public class LvlDetailsPanelController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private TextMesh titlePanel;

    [SerializeField]
    private GameObject objectiveText;

    [SerializeField]
    private GameObject PrizesObject;

    [SerializeField]
    private Image[] Mutators;
    
    [SerializeField]
    private Sprite[] MutatorRepresentation;

    private string[] I18NDetails = new string[2];

    private void OnEnable() {
        PrepareLevelDetailsPanelFromLevelSettings(GlobalVariables.Instance.actualLevelSettings);
    }
    
    public void GoToLevel(){
        GlobalVariables.Instance.UpdateGamemodeFromLvlSettings();
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
    }
}
