using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cesped : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("chocando contra cesped!");
        if(Object.Equals(collision.tag, "Player"))
        {
            collision.GetComponent<Player>().ContactedWithGrass();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            //Debug.Log("enters");
            Player pl = collision.GetComponent<Player>();
            if (pl != null) pl.life -= 1;
        }
    }

}
