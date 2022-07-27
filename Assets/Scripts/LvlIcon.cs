using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlIcon : MonoBehaviour
{
    [SerializeField]
    internal bool toggle = false;

    [SerializeField]
    internal Vector3 firstPos, secondaryPos;

    [SerializeField]
    internal LvlSettings lvlSettings;

    [SerializeField]
    private MeshFilter lvlStatus;

    void Start()
    {
        firstPos = transform.localPosition;
    }

    void FixedUpdate()
    {
        if(!toggle && firstPos != transform.localPosition)
            transform.localPosition = Vector3.Lerp(transform.localPosition, firstPos, Time.deltaTime * 7.0f); 
        else if(toggle && secondaryPos != transform.localPosition)
            transform.localPosition = Vector3.Lerp(transform.localPosition, secondaryPos, Time.deltaTime * 7.0f); 
    }

    internal void togglePosition(){
        toggle = !toggle;
    }

    internal void setLvlStatus(MeshFilter m, int status){
        lvlStatus.sharedMesh = m.sharedMesh;
        var ps_icon = lvlStatus.gameObject.GetComponent<ParticleSystem>();
        if(ps_icon != null){
            var colorOverLifetime = ps_icon.colorOverLifetime;
            colorOverLifetime.color = status == 0 ? Color.green : status == 1 ? Color.grey : Color.red;
        }
    }
}
