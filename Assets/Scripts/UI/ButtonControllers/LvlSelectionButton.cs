using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LvlSelectionButton : SelectButtonController
{

    public override void executeButton(){
        var levelSettings = GetComponent<LevelSettings>();
        if(levelSettings != null){
            GlobalVariables.Instance.PrepareGlobalToLevel(levelSettings, GameMode.INFINITERUNNER);
            GlobalVariables.Instance.actualPanelInteractionType = PanelInteractionType.NO_INTERACTION;
        }
        else Debug.LogError("LevelSettings not found!!");
    }
}
