using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnterWorldButtonController : SelectButtonController
{
    public override void executeButton(){
        StartCoroutine(LoadWorldOneScene());
    }

    private IEnumerator LoadWorldOneScene(){
        yield return new WaitForSeconds(4f);
        GlobalVariables.Instance.gameMode = GameMode.WOLRDMAINMENU;
        SceneManager.LoadScene(1);
    }

    private void OnEnable() {
        if(GlobalVariables.Instance != null) GlobalVariables.Instance.SetFocusUiElement(this.gameObject);
    }
}
