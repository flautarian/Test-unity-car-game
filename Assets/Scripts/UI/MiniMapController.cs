using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    
    public Transform reference;
    private Vector3 pos;
    private Vector3 rot;
    public Vector3 offset;
    void Start()
    {
        pos = transform.position;
        rot = new Vector3(0f, 0f, 0f);
        if(GlobalVariables.Instance.gameMode != GameMode.WOLRDMAINMENU) Destroy(this.gameObject);
    }

    // Update is called once per frame

    private void LateUpdate() { 
        rot.z = reference.rotation.eulerAngles.y*-1;
        rot.y = 0;
        rot.x = 90;
        pos.x = reference.position.x + offset.x;
        pos.z = reference.position.z + offset.z;
        transform.position = pos;
        transform.eulerAngles = rot;
    }
}
