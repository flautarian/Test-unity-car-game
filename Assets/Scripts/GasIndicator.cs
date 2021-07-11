using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasIndicator : MonoBehaviour
{
    public float currentTime;
    public float maxTime;
    private int timeToAdd;
    public bool gameStarted = false;
    public Transform indicator;
    public Vector3 maxIndicatorPosition;
    public Quaternion maxIndicatorRotation;
    public Vector3 minIndicatorPosition;
    public Quaternion minIndicatorRotation;

    internal void AddSeconds(float value)
    {
        timeToAdd += (int)value;
    }

    private void Start()
    {
        indicator.localPosition = minIndicatorPosition;
        indicator.localRotation = minIndicatorRotation;
        timeToAdd += (int)maxTime;
    }
    void Update()
    {
        if (gameStarted)
        {
            currentTime -= Time.deltaTime;
        }

        if (timeToAdd > 0)
        {
            var fractionOfTime = timeToAdd / 4;
            timeToAdd -= fractionOfTime;
            if (currentTime < 120) currentTime += fractionOfTime;
        }

        if (currentTime > 0)
        {
            float arrowPosition = (currentTime / maxTime);
            indicator.localPosition = Vector3.Lerp(minIndicatorPosition, maxIndicatorPosition, arrowPosition);
            indicator.localRotation = Quaternion.Lerp(minIndicatorRotation, maxIndicatorRotation, arrowPosition);
        }
        else
        {
            GameObject gui = GameObject.FindGameObjectWithTag("GUI");
            if (gui != null) gui.GetComponent<GUIController>().startGameOver("Gas Off!");
        }
    }

    public void startGameOver()
    {
        this.gameStarted = false;
        gameObject.SetActive(false);
        indicator.gameObject.SetActive(false);
    }

    public void startGame()
    {
        this.gameStarted = true;
    }
}
