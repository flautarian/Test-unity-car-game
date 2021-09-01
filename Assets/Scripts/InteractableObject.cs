
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    internal Vector3 initialLocalPosition;
    internal Quaternion initialLocalRotation;
    internal Vector3 initialLocalScale;
    internal Animator animator;

    private void Awake()
    {
        initialLocalPosition = transform.position;
        initialLocalRotation = transform.rotation;
        initialLocalScale = transform.lossyScale;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
        transform.localScale = initialLocalScale;
        animator.SetBool("hasBeenTaken", false);
    }

    public abstract void Execute();

    public void TakeObject()
    {
        if (!animator.GetBool("hasBeenTaken"))
        {
            animator.SetBool("hasBeenTaken", true);
            Execute();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(Equals(collision.gameObject.tag, "Player") || Equals(collision.gameObject.tag, "PlayerPart")) 
            TakeObject();
    }
}