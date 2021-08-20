
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public abstract void Execute(PlayerController controller);
    public void TakeObject(PlayerController controller)
    {
        if (!GetComponent<Animator>().GetBool("hasBeenTaken"))
        {
            GetComponent<Animator>().SetBool("hasBeenTaken", true);
            Execute(controller);
        }
    }
}