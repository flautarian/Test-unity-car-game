using Assets.Scripts;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public StreetType streetType = StreetType.asphalt;
    public GUIController guiPlayer;
    public bool grounded = false;
    public float comboStunt;
    internal Player player;
    private int actualCarEquipped = 0;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private StuntAnimationOverriderController stuntAnimationOverriderController;

    private GUIController guiController;

    private bool trickMode = false;

    private ParticleSystem.EmissionModule stuntComboPSEmissionVar;

    

    private Vector3 targetCorrectTurn = new Vector3(0f ,0f ,0f);

    [SerializeField]
    private StuntsController stuntsController;

    public float zAngle =0, xAngle =0;

    private void Awake(){
        player = GetComponentInChildren<Player>();
    }


    public void UpdateNitroAnimationState(bool state){
        playerAnimator.SetBool(Constants.ANIMATION_NAME_NITRO_BOOL, state);
    }

    /*public void SphereEnterCollides(Collision collision)
    {
        if (System.Object.Equals(collision.gameObject.layer, 8))// Ground
        {
            if(!grounded){
                GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_LANDINGCAR, transform.position);
                if(!System.Object.Equals(collision.gameObject.tag, Constants.WATER)) lastSecurePositionPlayer = transform.position;
                if(isStunting > -1){
                    ExecuteTurnUpCar();
                }
            }
            grounded = true;
            if (System.Object.Equals(collision.gameObject.tag, Constants.CESPED) && streetType != StreetType.grass){
                //slowedVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.STICKY_GRASS);
                //wetVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.WET_GRASS);
                streetType = StreetType.grass;
            }
            else if (System.Object.Equals(collision.gameObject.tag, Constants.ASPHALT) && streetType != StreetType.asphalt){
                //slowedVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.STICKY_ROAD);
                //wetVelocityPSEmissionVar.enabled = GlobalVariables.Instance.IsMutatorActive(Mutator.WET_ROAD);
                streetType = StreetType.asphalt;
            }
            else if (System.Object.Equals(collision.gameObject.tag, Constants.WATER))
            {
                streetType = StreetType.water;
                destroyPlayer(Constants.GAME_OVER_VEHICLE_DROWNED);
            }
        }
    }*/

    public void executeCarExplosionParticle()
    {
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_BOOM, transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawCube(com, new Vector3(0.25f, 0.25f, 0.25f));
    }


    public void ResetPlayerControllerRotation(){
        var rot = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);
        transform.rotation = rot;
    }

    private void ExecuteTurnUpCar(){
        playerAnimator.SetBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, true);
        var newRot = new Quaternion(transform.rotation.x, transform.rotation.y, 180, transform.rotation.w);
        transform.rotation = newRot;
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_TURNUPCAR, transform.position);
    }
}
