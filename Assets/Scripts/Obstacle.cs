using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Obstacle : MonoBehaviour
{

    public bool penalizableObstacle = true;

    public bool lethal = false;

    public int apparitionLevel;
    
    internal Animation animation;
    
    internal Rigidbody rigidBody;

    public Vector3 initialLocalPosition;

    public Quaternion initialLocalRotation;
    public abstract void SetPositioAndTargetFromSpawner(Spawner spawner);

    public abstract void Collide(Transform c);

    internal void destroyObstacle()
    {
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        initialLocalPosition = transform.position;
        initialLocalRotation = transform.rotation;
        rigidBody = GetComponent<Rigidbody>();
        animation = GetComponent<Animation>();
        this.gameObject.SetActive(GlobalVariables.Instance.totalCoins >= apparitionLevel);
    }

    private void OnEnable()
    {
        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
    }
}