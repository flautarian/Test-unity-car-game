using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject gasIndicator;
    public GameObject motorCarreteras;
    public Vector3 cameraOffset;
    public Vector3 cameraVelocityOffset;

    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float playerAcceleration = 0;
        float playerBrake = 0;
        if (player != null)
        {
            playerAcceleration = player.GetComponent<PlayerController>().VerticalAxis;
            playerBrake = player.GetComponent<PlayerController>().HorizontalAxis;
        }
        transform.LookAt(player.transform);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + cameraOffset.z * playerBrake);
    }

    public void startGame()
    {
        if(gasIndicator.GetComponent<GasIndicator>() != null)
            gasIndicator.GetComponent<GasIndicator>().startGame();

        if(player.GetComponent<PlayerController>() != null)
            player.GetComponent<PlayerController>().startGame();

        if(motorCarreteras.GetComponent<MotorCarreteras>() != null)
            motorCarreteras.GetComponent<MotorCarreteras>().startGame();
    }

    public void startGameOver(String msg)
    {
        Debug.LogWarning("game ended by: " + msg);
        if (gasIndicator.GetComponent<GasIndicator>() != null)
            gasIndicator.GetComponent<GasIndicator>().startGameOver();

        if (player.GetComponent<PlayerController>() != null)
            player.GetComponent<PlayerController>().startGameOver();

        if (motorCarreteras.GetComponent<MotorCarreteras>() != null)
            motorCarreteras.GetComponent<MotorCarreteras>().startGameOver();
    }
}
