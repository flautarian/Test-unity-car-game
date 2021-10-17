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
        SceneManager.LoadScene(1);
    }
}
