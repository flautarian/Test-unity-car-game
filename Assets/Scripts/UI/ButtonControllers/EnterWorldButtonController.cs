using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnterWorldButtonController : SelectButtonController
{
    public override void executeButton(){
        SceneManager.LoadScene(1);
    }
}
