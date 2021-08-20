using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Obstacle : MonoBehaviour
{

    public bool penalizableObstacle = true;

    public bool lethal = false;

    public int apparitionLevel = 0;
    public abstract void SetPositioAndTargetFromSpawner(Spawner spawner);

    public abstract void Collide(Transform c);

    internal void destroyObstacle()
    {
        Destroy(this.gameObject);
    }

    private void Start()
    {
        if (GlobalVariables.Instance.totalCoins < apparitionLevel) Destroy(this.gameObject);
    }
}