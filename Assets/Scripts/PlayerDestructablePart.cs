using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestructablePart : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController playerCar;
    private Rigidbody rbPart;
    internal bool destroyed = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (!destroyed)
        {
            if (collision.gameObject.tag.Contains("Obstaculo"))
            {
                playerCar.ComunicateCollisionPart(this.gameObject, collision);
            }
            else if (System.Object.Equals(collision.gameObject.tag, "PlayerInteractable"))
            {
                collision.gameObject.GetComponent<InteractableObject>().TakeObject(playerCar);
            }
        }
    }

    public void Update()
    {
        if (destroyed && transform.position.y < -1f) Destroy(this.gameObject);
    }

    internal void ejectPart(GameObject originalPart)
    {
        this.gameObject.name = originalPart.name + "-destroyed";
        transform.position = originalPart.transform.position;
        transform.rotation = originalPart.transform.rotation;
        rbPart = this.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
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
