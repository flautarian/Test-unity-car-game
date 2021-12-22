using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInteractionIconController : MonoBehaviour
{
    private Renderer rend;
    private float timeSentinelRaycast =0;
    void Start()
    {
        rend = GetComponent<Renderer>();       
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeSentinelRaycast >= 0.2f){
            rend.enabled = GlobalVariables.Instance.playerTargetedByCamera;
            timeSentinelRaycast = Time.time;
        }
    }
}
