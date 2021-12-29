using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenuCamera : MonoBehaviour
{

    public PlayerController playerController;

    private Vector3 offset;

    public Quaternion targetRotation;


    private void Start() {
        targetRotation = transform.localRotation;
        offset = transform.position - playerController.transform.position;
    }
    
    void LateUpdate (){
        transform.LookAt(playerController.transform, Vector3.up);
        //if(Input.GetAxis(Constants.INPUT_ACCELERATE) != 0) transform.position = Vector2.Lerp(transform.position, player.transform.position + offset, 5 * Time.deltaTime);
        //else 
        transform.RotateAround(playerController.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
