using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nitro : InteractableObject
{
    private GUIController guiController;

    void Start()
    {
        var guigo = GameObject.FindGameObjectWithTag("GUI");
        if(guigo != null)
            guiController = guigo.GetComponent<GUIController>();
    }

    public override void Execute()
    {
        if(guiController != null)
            guiController.AddNitro();
    }
}
