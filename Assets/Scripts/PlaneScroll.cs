using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScroll : MonoBehaviour
{
    // Update is called once per frame
    public Vector2 scroll;
    private Renderer scrollRenderer;
    private CarController carController;
    public Vector3 playerOffset;
    private Vector3 waterPosition;
    private void Start()
    {
        scrollRenderer = GetComponent<Renderer>();
        var objPlayer = GameObject.FindGameObjectWithTag(Constants.GO_TAG_PLAYER);
        if (objPlayer != null && objPlayer.TryGetComponent(out Player p))
            carController = p.carController;
    }
    void Update()
    {
        if(playerOffset != Vector3.zero){
            waterPosition = carController.transform.position + playerOffset;
            if (waterPosition.y > -2 || waterPosition.y < -2) waterPosition.y = -2;
            transform.position = waterPosition;
        }
        scroll.y = carController.GetVerticalAxis() * (GlobalVariables.Instance.gameMode == GameMode.INFINITERUNNER ? 0.25f : 0f);
        scrollRenderer.sharedMaterial.SetVector("Vector2_E3542F48", scroll);
    }
}
