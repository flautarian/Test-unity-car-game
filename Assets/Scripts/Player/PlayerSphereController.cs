using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class PlayerSphereController : MonoBehaviour
    {

        // Start is called before the first frame update
        public PlayerController playerController;

        void OnCollisionEnter(Collision c){
            playerController.SphereEnterCollides(c);
        }
        
    }
}