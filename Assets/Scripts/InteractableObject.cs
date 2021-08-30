
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    Vector3 initialPos;

    private void Awake()
    {
        initialPos = transform.localPosition;
    }
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