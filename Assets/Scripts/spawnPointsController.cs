using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPointsController : MonoBehaviour
{
    
    public GameObject mainCamera;
    Vector3 _followOffset;
    public float followSharpness = 0.1f;


    void Start()
    {
        // preparamos la distancia entre los puntos de spawn y la camara
        _followOffset = transform.position - mainCamera.transform.position;
    }

    
    void Update()
    {
        // actualizamos para que siempre haya la misma distancia entre la camara y el spawn, asi la camara nunca pillara al spawn que tiene arriba
        LateUpdate();
    }


    void LateUpdate()
    {
        // Apply that offset to get a target position.
        Vector3 targetPosition = mainCamera.transform.position + _followOffset;

        // Keep our y position unchanged.
        targetPosition.x = transform.position.x;

        // Smooth follow.    
        transform.position += (targetPosition - transform.position) * followSharpness;
    }
}
