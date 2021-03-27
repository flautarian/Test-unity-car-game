using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cesped : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() != null)
        {
            //Debug.Log("stay");
            Player pl = collision.collider.GetComponent<Player>();
            if (pl != null) pl.life -= 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<Player>() != null)
        {
            //Debug.Log("enters");
            Player pl = collision.collider.GetComponent<Player>();
            if (pl != null) pl.life -= 1;
        }
    }

}
