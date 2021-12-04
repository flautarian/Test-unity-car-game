using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInteractionIconController : MonoBehaviour
{
    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();       
    }

    // Update is called once per frame
    void Update()
    {
        rend.enabled = GlobalVariables.Instance.inGameState == InGamePanels.GAMEON;
    }
}
