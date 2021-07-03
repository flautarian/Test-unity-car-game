using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasIndicator : MonoBehaviour
{
    public float currentTime;
    public float maxTime;
    public bool gameStarted = false;
    public Transform indicator;
    public Vector3 maxIndicatorPosition;
    public Quaternion maxIndicatorRotation;
    public Vector3 minIndicatorPosition;
    public Quaternion minIndicatorRotation;

    internal void AddSeconds(float value)
    {
        currentTime += value;
        if (currentTime > 120) currentTime = 120;
    }

    private void Start()
    {
        indicator.localPosition = minIndicatorPosition;
        indicator.localRotation = minIndicatorRotation;
    }
    void Update()
    {
        if (gameStarted) {
            currentTime -= Time.deltaTime;     
            if (currentTime > 0)
            {
                float arrowPosition = (currentTime / maxTime);
                indicator.localPosition = Vector3.Lerp(minIndicatorPosition, maxIndicatorPosition, arrowPosition);
                indicator.localRotation = Quaternion.Lerp(minIndicatorRotation, maxIndicatorRotation, arrowPosition);
            }
            else
            {
                GameObject gui = GameObject.FindGameObjectWithTag("GUI");
                if (gui != null) gui.GetComponent<GUIPlayer>().startGameOver("Gas Off!");
            }
        }
    }

    public void startGameOver()
    {
        this.gameStarted = false;
    }

    public void startGame()
    {
        this.gameStarted = true;
    }
}
