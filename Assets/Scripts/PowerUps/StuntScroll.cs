using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntScroll : InteractableObject
{
    // Start is called before the first frame update

    public Vector3 animPosition;
    public int indexScroll;

    public Mesh inactiveScrollMesh;

    private void Start() {
        if(GlobalVariables.Instance.IsScrollEnabled(indexScroll)){
            MeshFilter m = GetComponent<MeshFilter>();
            if(m != null)
                m.sharedMesh = inactiveScrollMesh;
        }
    }

    void LateUpdate()
    {
        if (!animator.GetBool(Constants.ANIMATION_NAME_TAKEN_BOOL)) return;
        transform.localPosition += animPosition;
    }

    public override void Execute()
    {
        GlobalVariables.Instance.UnlockScroll(indexScroll);
    }
}
