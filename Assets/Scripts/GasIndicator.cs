using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasIndicator : MonoBehaviour
{
    public float currentTime;
    public float maxTime;
    private int timeToAdd;
    private Animation arrowAnimation;
    public bool gameStarted = false;
    public Transform indicator;
    public Vector3 maxIndicatorPosition;
    public Quaternion maxIndicatorRotation;
    public Vector3 minIndicatorPosition;
    public Quaternion minIndicatorRotation;

    private static float MIN_GAS = 20;

    internal void AddSeconds(float value)
    {
        timeToAdd += (int)value;
    }

    private void Start()
    {
        indicator.localPosition = minIndicatorPosition;
        indicator.localRotation = minIndicatorRotation;
        timeToAdd += (int)maxTime;
        arrowAnimation = GetComponent<Animation>();
    }
    void Update()
    {
        if (gameStarted)
        {
            currentTime -= Time.deltaTime;

            if (arrowAnimation != null)
            {
                if (currentTime < MIN_GAS && !arrowAnimation.isPlaying)
                    arrowAnimation.Play();
                else if (currentTime > MIN_GAS)
                {
                    arrowAnimation.Rewind();
                    if(arrowAnimation.isPlaying) arrowAnimation.Stop();
                }
            }
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
