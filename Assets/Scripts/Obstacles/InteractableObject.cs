
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public Vector3 initialLocalPosition;
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
        if(GlobalVariables.Instance != null && GlobalVariables.Instance.gameMode == GameMode.INFINITERUNNER){
            transform.localPosition = initialLocalPosition;
            transform.localRotation = initialLocalRotation;
            transform.localScale = initialLocalScale;
            animator.SetBool(Constants.ANIMATION_NAME_TAKEN_BOOL, false);
        }
    }

    public abstract void Execute();

    public void TakeObject()
    {
        if (!animator.GetBool(Constants.ANIMATION_NAME_TAKEN_BOOL))
        {
            animator.SetBool(Constants.ANIMATION_NAME_TAKEN_BOOL, true);
            Execute();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(Equals(collision.gameObject.tag, Constants.GO_TAG_PLAYER) || Equals(collision.gameObject.tag, Constants.GO_TAG_PLAYER_PART)) 
            TakeObject();
    }
}