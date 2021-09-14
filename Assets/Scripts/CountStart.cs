using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountStart : MonoBehaviour
{
    public Sprite[] spriteArray;
    public AudioClip[] audioClips;
    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void StartGame()
    {
        GameObject timer = GameObject.FindGameObjectWithTag(Constants.GO_TAG_GUI);
        if (timer != null) timer.GetComponent<GUIController>().startGame();
    }
}
