using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenuCamera : MonoBehaviour
{

    public Player player;

    private Vector3 offset;

    public Quaternion targetRotation;


    private void Start() {
        targetRotation = transform.localRotation;
        offset = transform.position - player.transform.position;
    }
    
    void LateUpdate (){
        transform.LookAt(player.transform, Vector3.up);
        //if(Input.GetAxis(Constants.INPUT_ACCELERATE) != 0) transform.position = Vector2.Lerp(transform.position, player.transform.position + offset, 5 * Time.deltaTime);
        //else 
        transform.RotateAround(player.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
