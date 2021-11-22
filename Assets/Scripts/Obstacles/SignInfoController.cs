using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInfoController : MonoBehaviour
{
    private Renderer rend;
    private bool pointed; 
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
        rend = GetComponent<Renderer>();
    }

    private void Update() {
        if(pointed){
            transform.LookAt(cam.transform);
            rend.enabled = true;
        }
        else {
            rend.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other){
        if(Equals(other.gameObject.tag, Constants.GO_TAG_PLAYER)){
            pointed = true;
            GlobalVariables.Instance.updateMainCameraLookAt(this.transform);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(Equals(other.gameObject.tag, Constants.GO_TAG_PLAYER)){
            pointed = false;
            GlobalVariables.Instance.updateMainCameraLookAt(null);
        }
    }
}
