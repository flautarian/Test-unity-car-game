using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cesped : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        
        if(Object.Equals(collision.tag, "Player"))
        {
            Debug.Log("estando contra cesped!");
            collision.GetComponent<Player>().ContactedWithGrass();
        }
    }


}
