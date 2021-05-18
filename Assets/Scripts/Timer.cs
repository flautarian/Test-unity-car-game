using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float currentTime;
    public float currentDistance = 0;

    public bool startCount = false;

    public PlayerController player;
    public GameObject distance;
    public GameObject time;
    private void Start()
    {
    }
    void Update()
    {
        if (startCount) {
            currentTime -= Time.deltaTime; 
            currentDistance += player.playerRigidbody.velocity.z;        
        }
        
        time.GetComponent<UnityEngine.UI.Text>().text = displayTime();
        distance.GetComponent<UnityEngine.UI.Text>().text = ((int)currentDistance).ToString();
        if (currentTime < 0) setGameOver();
    }

    private void setGameOver()
    {
        this.startCount = false;
        player.EndGame();
    }

    private string displayTime()
    {
        return "" + displayMinutes() + ":" + displaySeconds();
    }
    public void startCountTime()
    {
        this.startCount = true;
        player.StartGame();
    }

    float displaySeconds()
    {
        return Mathf.FloorToInt(currentTime % 60);
    }

    float displayMinutes()
    {
        return Mathf.FloorToInt(currentTime / 60);
    }
}
