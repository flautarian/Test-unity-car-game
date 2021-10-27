using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LvlSelectionButton : SelectButtonController
{

    public override void executeButton(){
        StartCoroutine(LoadLvlSettingsAndGoScene());
    }

    private IEnumerator LoadLvlSettingsAndGoScene(){
        var levelSettings = GetComponent<LevelSettings>();
        if(levelSettings != null){
            GlobalVariables.Instance.PrepareGlobalToLevel(levelSettings, GameMode.INFINITERUNNER);
            yield return new WaitForSeconds(4f);
            SceneManager.LoadScene(2);
        }
    }
}
