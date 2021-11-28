using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsButtonController : SelectButtonController
{
    public override void executeButton(){
        EventSystem.current.SetSelectedGameObject(null);
    }
}
