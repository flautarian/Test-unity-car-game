using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountStart : MonoBehaviour
{
    public Sprite[] spriteArray;
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(backCountTime());
    }

    private IEnumerator backCountTime()
    {

        audioSource.Play();

        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = spriteArray[1];
        audioSource.Play();

        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = spriteArray[0];
        audioSource.Play();

        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = spriteArray[3];
        audioSource.clip = audioClips[1];
        audioSource.Play();

        GameObject timer = GameObject.FindGameObjectWithTag(Constants.GO_TAG_GUI);
        if (timer != null) timer.GetComponent<GUIController>().startGame();
        yield return new WaitForSeconds(2);
        spriteRenderer.enabled = false;
        Destroy(this.gameObject);
    }
}
