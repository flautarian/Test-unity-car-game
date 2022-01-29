using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScroll : MonoBehaviour
{
    // Update is called once per frame
    public Vector2 scroll;
    private Renderer scrollRenderer;
    [SerializeField]
    private PlayerController playerController;
    public Vector3 playerOffset;
    private Vector3 waterPosition;
    private void Start()
    {
        scrollRenderer = GetComponent<Renderer>();
    }
    void Update()
    {
        if(playerOffset != Vector3.zero){
            waterPosition = playerController.transform.position + playerOffset;
            if (waterPosition.y > -2 || waterPosition.y < -2) waterPosition.y = -2;
            transform.position = waterPosition;
        }
        scroll.y = playerController.GetVerticalAxis() * (GlobalVariables.Instance.gameMode == GameMode.INFINITERUNNER ? 0.25f : 0f);
        scrollRenderer.sharedMaterial.SetVector("Vector2_E3542F48", scroll);
    }
}
