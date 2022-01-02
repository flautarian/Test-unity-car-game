using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : InteractableObject
{
    // Start is called before the first frame update

    public Vector3 animPosition;
    public int indexRelic;

    private void Start() {
        if(GlobalVariables.Instance.IsRelicEnabled(indexRelic))
            Destroy(this.gameObject);
    }
    void LateUpdate()
    {
        if (!animator.GetBool(Constants.ANIMATION_NAME_TAKEN_BOOL)) return;
        transform.localPosition += animPosition;
    }

    public override void Execute()
    {
        GlobalVariables.Instance.UnlockRelic(indexRelic);
    }
}
