using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class PlayerSphereController : MonoBehaviour
    {

        // Start is called before the first frame update
        public PlayerController playerController;

        private void OnCollisionEnter(Collision collision)
        {
            if(playerController != null) playerController.SphereEnterCollides(collision);
        }
    }
}