using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisicButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    GUIController guiController;
    public ActionButtonType actionButton;
    void Start()
    {
        guiController = GameObject.FindGameObjectWithTag("GUI").GetComponent<GUIController>();
    }

    // Update is called once per frame
    void execute()
    {
        if(guiController != null)
        {
            guiController.propagueFisicButton(this);
        }
    }
}
