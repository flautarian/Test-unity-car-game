using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWheel : MonoBehaviour
{
    
    [SerializeField]
    internal Mesh CWheel;
    
    [SerializeField]
    internal int keyCode = -1;

    [SerializeField]
    internal int price;

    [SerializeField]
    internal float wheelOffset = 0f;
    
    [SerializeField]
    internal float wheelSize = 1f;
}
