using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LvlSelectionButton : SelectButtonController
{

    [SerializeField]
    private Sprite[] lvlStatus;

    private Image lvlIcon;
    
    [SerializeField]
    private int lvlIndex;
    private LevelSettings lvlSettings;

    private void Start() {
        lvlSettings = GetComponent<LevelSettings>();
        lvlIcon = GetComponent<Image>();
        if(GlobalVariables.Instance.IsCompletedLevel(lvlSettings.lvlIndex))
            lvlIcon.sprite = lvlStatus[2];
        else if(GlobalVariables.Instance.IsCompletableLevel(lvlSettings.lvlIndex))
            lvlIcon.sprite = lvlStatus[1];
        else 
            lvlIcon.sprite = lvlStatus[0];
    }

    public override void executeButton(){
        LevelSettings levelSettings = GetComponent<LevelSettings>();
        if(levelSettings != null){
            GlobalVariables.Instance.PrepareGlobalToLevel(levelSettings);
        }
        else Debug.LogError("LevelSettings not found!!");
        
    }
    
}
