using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wheel", menuName = "VoxelRacer/Wheel", order = 0)]
public class ShopWheel : ScriptableObject {
    
    [SerializeField]
    internal Mesh CWheel;
    
    [SerializeField]
    internal int keyCode = -1;

    [SerializeField]
    internal int price = 0;

    [SerializeField]
    internal float wheelOffset = 0f;

    [SerializeField]
    internal float wheelSize = 1f;

    [SerializeField]
    internal float wheelMass = 25f;

    [SerializeField]
    internal float wheelForwardFrictionExtremumSlip = 0.4f;

    [SerializeField]
    internal float wheelSidewaysFrictionExtremumSlip = 0.3f;

    public ShopWheel(){

    }
    public ShopWheel(ShopWheel w){
        this.CWheel = w.CWheel;
        this.price = w.price;
        this.keyCode = w.keyCode;
        this.wheelSize = w.wheelSize;
        this.wheelOffset = w.wheelOffset;
        this.wheelMass = w.wheelMass;
        this.wheelForwardFrictionExtremumSlip = w.wheelForwardFrictionExtremumSlip;
        this.wheelSidewaysFrictionExtremumSlip = w.wheelSidewaysFrictionExtremumSlip;
    }
}
