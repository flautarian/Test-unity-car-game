using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180;
    // Start is called before the first frame update
    private void Start()
    {
        rigidBody.transform.parent = null;
    }


    void Update()
    {
        if (rigidBody != null) transform.position = rigidBody.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (rigidBody != null) rigidBody.AddForce(transform.forward * forwardAccel);
    }
}
