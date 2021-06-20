using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScroll: MonoBehaviour
{
    // Update is called once per frame
    public Vector2 scroll;
    private Renderer renderer;
    public PlayerController playerController;
    public Vector3 playerOffset;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    void Update()
    {
        scroll.y = playerController.VerticalAxis*0.25f;
        var pos = playerController.transform.position + playerOffset;
        if (pos.y > -2) pos.y = -2;
        transform.position = pos;
        renderer.sharedMaterial.SetVector("Vector2_E3542F48", scroll);
    }
}
