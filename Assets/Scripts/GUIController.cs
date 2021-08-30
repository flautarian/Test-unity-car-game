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
    public bool forceStartGame = false;

    private float cameraXAxisOffset = 20;
    private float cameraYAxisOffset = 5;

    private void Start()
    {
        // init de controlador de partes de coche;
        if (carPartsIndicator.GetComponent<CarPartsIndicator>() != null)
            carPartsIndicator.GetComponent<CarPartsIndicator>().startGame(player.GetComponent<PlayerController>().destructableParts.Count);
        // force startGame for edit actions
        if (forceStartGame) startGame();
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
        //transform.LookAt(player.transform);
        transform.rotation = Quaternion.Euler(transform.rotation.x + (cameraXAxisOffset * playerAcceleration), transform.rotation.y + (cameraYAxisOffset * playerBrake), transform.rotation.z);
    }

    public void startGame()
    {
        if (cmvStateDriveCameraAnimator != null)
            cmvStateDriveCameraAnimator.Play("MainGameCamera");

        if (gasIndicator != null && gasIndicator.GetComponent<GasIndicator>() != null)
            gasIndicator.GetComponent<GasIndicator>().startGame();

        if(player != null && player.GetComponent<PlayerController>() != null)
            player.GetComponent<PlayerController>().startGame();        

        if (motorCarreteras != null && motorCarreteras.GetComponent<MotorCarreteras>() != null)
            motorCarreteras.GetComponent<MotorCarreteras>().startGame();

        if (coinsIndicator != null && coinsIndicator.GetComponent<CoinsIndicator>() != null)
            coinsIndicator.GetComponent<CoinsIndicator>().startGame();

    }

    internal void addCoins(int number)
    {
        coinsIndicator.GetComponent<CoinsIndicator>().addCoins(number);
    }

    public void startGameOver(String msg)
    {
        Debug.LogWarning("game ended by: " + msg);
        if (gasIndicator != null && gasIndicator.GetComponent<GasIndicator>() != null)
            gasIndicator.GetComponent<GasIndicator>().startGameOver();

        if (player != null && player.GetComponent<PlayerController>() != null)
            player.GetComponent<PlayerController>().startGameOver();

        if (motorCarreteras != null && motorCarreteras.GetComponent<MotorCarreteras>() != null)
            motorCarreteras.GetComponent<MotorCarreteras>().startGameOver();

        if (coinsIndicator != null && coinsIndicator.GetComponent<CoinsIndicator>() != null)
            coinsIndicator.GetComponent<CoinsIndicator>().startGameOver();

        if (carPartsIndicator != null && carPartsIndicator.GetComponent<CarPartsIndicator>() != null)
            carPartsIndicator.GetComponent<CarPartsIndicator>().startGameOver();

        if (cmvStateDriveCameraAnimator != null)
            cmvStateDriveCameraAnimator.Play("GameOverCamera");
    }
    
}