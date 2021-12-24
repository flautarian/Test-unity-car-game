using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcessionaryCar : MonoBehaviour
{
    
    [SerializeField]
    internal Mesh CCar;
    
    [SerializeField]
    internal int keyCode = -1;
    
    [SerializeField]
    internal Player playerInfoClass;

    [SerializeField]
    internal int price;

    [SerializeField]
    internal string carName;
}
