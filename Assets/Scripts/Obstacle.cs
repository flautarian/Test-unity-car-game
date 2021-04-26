using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Obstacle : MonoBehaviour
{
    public abstract void SetPathFromSpawner(Spawner spawner);
}