using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Obstacle : MonoBehaviour
{

    public bool penalizableObstacle = true;
    public abstract void SetPositioAndTargetFromSpawner(Spawner spawner);

    public abstract void Collide(Transform c);
}