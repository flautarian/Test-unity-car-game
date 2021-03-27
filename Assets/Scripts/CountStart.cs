using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountStart : MonoBehaviour
{
    public Sprite[] spriteArray;
    public AudioClip[] audioClips;
    public GameObject motor;

    private void Start()
    {
        if (motor != null) StartCoroutine(backCountTime());
    }

    private IEnumerator backCountTime()
    {

        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().sprite = spriteArray[1];
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().sprite = spriteArray[0];
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().sprite = spriteArray[3];
        GetComponent<AudioSource>().clip = audioClips[1];
        GetComponent<AudioSource>().Play();

        motor.GetComponent<MotorCarreteras>().inicioJuego = true;
        yield return new WaitForSeconds(2);
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(this.gameObject);
    }
}
