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
    private Vector3 waterPosition;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    void Update()
    {
        waterPosition = playerController.transform.position + playerOffset;
        if (waterPosition.y > -2 || waterPosition.y < -2) waterPosition.y = -2;
        transform.position = waterPosition;
        if(playerController.VerticalAxis == 0 || playerController.VerticalAxis == 1)
        {
            scroll.y = playerController.VerticalAxis * 0.25f;
            renderer.sharedMaterial.SetVector("Vector2_E3542F48", scroll);
        }
    }
}
