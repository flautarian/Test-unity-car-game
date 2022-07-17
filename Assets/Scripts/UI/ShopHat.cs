using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHat : MonoBehaviour
{
    
    [SerializeField]
    internal Mesh CHat;
    
    [SerializeField]
    public int keyCode = -1;

    [SerializeField]
    internal int price;

    public ShopHat(){
        
    }

    public void CopyHat(ShopHat h){
        CHat = h.CHat;
        keyCode = h.keyCode;
        price = h.price;
    }
}
