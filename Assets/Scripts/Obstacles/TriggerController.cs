using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    internal bool trigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Constants.GO_TAG_PLAYER)
            trigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == Constants.GO_TAG_PLAYER)
            trigger = false;
    }
}
