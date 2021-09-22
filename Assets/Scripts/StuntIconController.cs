using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntIconController : MonoBehaviour
{
    internal Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
}
