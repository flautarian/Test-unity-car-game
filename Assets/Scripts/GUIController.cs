using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public GameObject player;
    public GameObject gasIndicator;
    public GameObject motorCarreteras;
    public GameObject coinsIndicator;
    public GameObject carPartsIndicator;
    public Animator cmvStateDriveCameraAnimator;

    private void Start()
    {

        // init de controlador de partes de coche;
        if (carPartsIndicator.GetComponent<CarPartsIndicator>() != null)
            carPartsIndicator.GetComponent<CarPartsIndicator>().startGame(player.GetComponent<PlayerController>().destructableParts.Count);
    }

    internal void propagueFisicButton(FisicButtonController fisicButtonController)
    {
        switch (fisicButtonController.actionButton)
        {
            case ActionButtonType.left:
                player.GetComponent<PlayerController>().turnLeft();
                break;
            case ActionButtonType.right:
                player.GetComponent<PlayerController>().turnRight();
                break;
            case ActionButtonType.brake:
                break;
        }
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
        if (cmvStateDriveCameraAnimator != null)
            cmvStateDriveCameraAnimator.Play("MainGameCamera");

        if (gasIndicator.GetComponent<GasIndicator>() != null)
            gasIndicator.GetComponent<GasIndicator>().startGame();

        if(player.GetComponent<PlayerController>() != null)
            player.GetComponent<PlayerController>().startGame();        

        if (motorCarreteras.GetComponent<MotorCarreteras>() != null)
            motorCarreteras.GetComponent<MotorCarreteras>().startGame();

        if (coinsIndicator.GetComponent<CoinsIndicator>() != null)
            coinsIndicator.GetComponent<CoinsIndicator>().startGame();

    }

    internal void addCoins(int number)
    {
        coinsIndicator.GetComponent<CoinsIndicator>().addCoins(number);
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

        if (coinsIndicator.GetComponent<CoinsIndicator>() != null)
            coinsIndicator.GetComponent<CoinsIndicator>().startGameOver();

        if (carPartsIndicator.GetComponent<CarPartsIndicator>() != null)
            carPartsIndicator.GetComponent<CarPartsIndicator>().startGameOver();

        if (cmvStateDriveCameraAnimator != null)
            cmvStateDriveCameraAnimator.Play("GameOverCamera");
    }
    
}