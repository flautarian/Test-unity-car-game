using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLineController : MonoBehaviour
{
    private bool goalPassed = false;

    [SerializeField]
    private ParticleSystem[] particles;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == Constants.GO_TAG_PLAYER){
            if(GlobalVariables.Instance.generateGoalLine){
                Player player = other.GetComponent<Player>();
                if(player != null){
                    player.StartGameWon();
                    foreach(ParticleSystem p in particles){
                        p.Play();
                    }
                }
            }
            else {
                GlobalVariables.Instance.NotifyToUISystem("^notif_goal_not_unlocked");
                GlobalVariables.Instance.GetAndPlayChunk("UI_Ko", 1.0f);
            }
        }
            
    }
}
