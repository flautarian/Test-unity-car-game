using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToWorldButtonController : SelectButtonController
{
    public override void executeButton(){
        GlobalVariables.Instance.UpdateLevelState(InGamePanels.GAMEON);
    }
}
