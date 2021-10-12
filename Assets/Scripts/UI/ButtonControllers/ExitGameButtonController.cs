using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameButtonController : SelectButtonController
{
    public override void executeButton(){
        Application.Quit();
    }
}
