using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsIndicator : MonoBehaviour
{
    private bool gameStarted = false;
    private int coins;
    public numberMeshGenerator numberIndicator;

    private void Update()
    {
        if (gameStarted)
        {
            int externalCoins = GlobalVariables.Instance.totalCoins;
            if(externalCoins != coins)
            {
                coins = externalCoins;
                if (numberIndicator != null) numberIndicator.updateNumber(coins);
            }
        }
    }

    public void startGame()
    {
        coins = 0;
        gameStarted = true;
    }

    public void startGameOver()
    {
        gameStarted = false;
        gameObject.SetActive(false);
    }
}
