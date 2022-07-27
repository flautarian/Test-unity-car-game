using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Honeti;
using System;

public class BuildingController : MonoBehaviour
{
    public static event Action<InGamePanels, System.Object> OnChangeGameState = delegate { };

    internal void executeChangeState(InGamePanels state, System.Object lvl){
        OnChangeGameState(state, lvl);
    }
}
