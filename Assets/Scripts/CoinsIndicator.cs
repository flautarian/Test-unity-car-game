using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsIndicator : MonoBehaviour
{
    private bool gameStarted = false;
    private int coins;
    public numberMeshGenerator numberIndicator;

    public void addCoins(int quantity)
    {
        if (gameStarted)
        {
            coins += quantity;
            if (numberIndicator != null) numberIndicator.updateNumber(coins);
        }
    }

    public void startGame()
    {
        gameStarted = true;
    }

    public void startGameOver()
    {
        gameStarted = false;
        gameObject.SetActive(false);
    }
}