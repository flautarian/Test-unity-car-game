using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Obstacle : MonoBehaviour
{

    public bool penalizableObstacle = true;

    public bool lethal = false;

    public int apparitionLevel;

    internal Animator animator;

    internal Rigidbody rigidBody;

    internal Vector3 initialLocalPosition;

    internal Quaternion initialLocalRotation;

    public abstract void SetPositioAndTargetFromSpawner(Spawner spawner);

    public abstract void Collide(Transform c);

    internal void destroyObstacle()
    {
        Destroy(this.gameObject);
    }

    internal void inhabiliteObstacle()
    {
        this.gameObject.SetActive(false);
    }

    private void Awake()
    {
        StartCoroutine(initializeObstacle());
    }

    private void OnEnable()
    {
        if (GlobalVariables.Instance != null)
        {
            transform.localPosition = initialLocalPosition;
            transform.localRotation = initialLocalRotation;
        }
    }

    private IEnumerator initializeObstacle()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        while (GlobalVariables.Instance == null) yield return null;
        this.gameObject.SetActive(GlobalVariables.Instance.totalCoins >= apparitionLevel);
    }
}