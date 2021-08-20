using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpButton : MonoBehaviour
{
    public Sprite alterSprite;
    public string powerUpName;
    //private bool enabled = false;
    public void enable()
    {
        switchSprite();
        enabled = true;
    }

    private void switchSprite()
    {
        Button b = GetComponent<Button>();
        Sprite cSprite = b.GetComponent<Image>().sprite;
        b.GetComponent<Image>().sprite = alterSprite;
        alterSprite = cSprite;
    }

    public void execute()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            player.GetComponent<Player>().executePowerUp(this.name);
        }
    }
}
