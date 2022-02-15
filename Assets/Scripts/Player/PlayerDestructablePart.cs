using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestructablePart : MonoBehaviour
{
    // Start is called before the first frame update
    private Player playerCar;
    private Rigidbody rbPart;
    internal bool destroyed = false;
    private Collider collider;

    private void Start() {
        var objPlayer = GameObject.FindGameObjectWithTag(Constants.GO_TAG_PLAYER);
        if (objPlayer.TryGetComponent(out Player p))
            playerCar = p;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!destroyed)
        {
            if (collision.gameObject.tag.Contains(Constants.GO_TAG_CONTAINS_OBSTACULO))
            {
                playerCar.ComunicateCollisionPart(this, collision);
            }
        }
    }

    public void Update()
    {
        if ( transform.position.y < -1f && this.name.Contains("-destroyed")) Destroy(this.gameObject);
    }

    internal void ejectPart()
    {
        GameObject falseDestroyPart = Instantiate(this.gameObject);
        falseDestroyPart.transform.parent = null;
        falseDestroyPart.name = this.name + "-destroyed";
        falseDestroyPart.transform.position = transform.position;
        falseDestroyPart.transform.rotation = transform.rotation;
        rbPart = falseDestroyPart.AddComponent(typeof(Rigidbody)) as Rigidbody;
        if(rbPart != null)
        {
            rbPart.useGravity = true;
            rbPart.mass = 10;
            rbPart.AddForce(Vector3.up * 3000f);
        }
    }

    internal void Inhabilite()
    {
        this.GetComponent<Renderer>().enabled =false;
        destroyed = true;
    }

    internal void Recover()
    {
        this.GetComponent<Renderer>().enabled = true;
        destroyed = false;
    }
}
