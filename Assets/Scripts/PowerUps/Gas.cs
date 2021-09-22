using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : InteractableObject
{

    public Vector3 animPosition;
    public float value;
    private GUIController guiController;

    void Start()
    {
        var go = GameObject.FindGameObjectWithTag(Constants.GO_TAG_GUI);
        if(go != null) guiController = go.GetComponent<GUIController>();
    }

    void LateUpdate()
    {
        if (!animator) return;
        transform.localPosition += animPosition;
    }
    public override void Execute()
    {
        if(guiController != null) 
            guiController.AddSeconds(value);
    }
}
