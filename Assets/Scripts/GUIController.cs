using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public PlayerController playerController;
    public GasIndicator gasIndicator;
    public CoinsIndicator coinsIndicator;
    public CarPartsIndicator carPartsIndicator;
    public Animator cmvStateDriveCameraAnimator;
    public bool forceStartGame = false;

    private float cameraXAxisOffset = 20;
    private float cameraYAxisOffset = 5;

    private void Start()
    {
        // init de controlador de partes de coche;
        if (carPartsIndicator != null)
            carPartsIndicator.startGame(playerController.destructableParts.Count);

        // force startGame for edit actions
        if (forceStartGame) startGame();
    }

    internal void propagueFisicButton(FisicButtonController fisicButtonController)
    {
        switch (fisicButtonController.actionButton)
        {
            case ActionButtonType.left:
                playerController.turnLeft();
                break;
            case ActionButtonType.right:
                playerController.turnRight();
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
        if (playerController != null)
        {
            playerAcceleration = playerController.VerticalAxis;
            playerBrake = playerController.HorizontalAxis;
        }
        //transform.LookAt(player.transform);
        transform.rotation = Quaternion.Euler(transform.rotation.x + (cameraXAxisOffset * playerAcceleration), transform.rotation.y + (cameraYAxisOffset * playerBrake), transform.rotation.z);
    }

    public void startGame()
    {
        if (cmvStateDriveCameraAnimator != null)
            cmvStateDriveCameraAnimator.Play("MainGameCamera");

        if (gasIndicator != null)
            gasIndicator.startGame();

        if(playerController != null)
            playerController.startGame();        

        if (coinsIndicator)
            coinsIndicator.startGame();

    }

    internal void addCoins(int number)
    {
        coinsIndicator.addCoins(number);
    }

    public void startGameOver(String msg)
    {
        Debug.LogWarning("game ended by: " + msg);
        if (gasIndicator != null)
            gasIndicator.startGameOver();

        if (playerController != null)
            playerController.startGameOver();

        if (coinsIndicator != null)
            coinsIndicator.startGameOver();

        if (carPartsIndicator != null )
            carPartsIndicator.startGameOver();

        if (cmvStateDriveCameraAnimator != null)
            cmvStateDriveCameraAnimator.Play("GameOverCamera");
    }

    public void RecoverCarPlayerParts()
    {
        if(playerController != null)
            playerController.RecoverParts();
        if(carPartsIndicator != null )
            carPartsIndicator.resetIndicator();
    }

    public void AddNitro()
    {
        if(playerController != null)
            playerController.AddNitro();
    }
    
}