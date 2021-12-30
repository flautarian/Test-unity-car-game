using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetiveContainerController : MonoBehaviour
{
    [SerializeField]
    private Transform ObjectiveIndicatorTransform;

    private Renderer Rend;

    private int actualQuant =1, objectiveQuant =2;
    private Vector3 finalMark, tempMark;
    Animation anim;
    void Start()
    {
        if(!GlobalVariables.Instance.IsLevelGameState())
            Destroy(this.gameObject);

        Rend = GetComponent<Renderer>();
        anim = GetComponent<Animation>();
        objectiveQuant = GlobalVariables.Instance.GetActualObjectiveTarget();
        
        finalMark = new Vector3(-1.35f, 0f, 0.3f);
        tempMark = new Vector3(0f, 0f, 0f);
        ObjectiveIndicatorTransform.localPosition = new Vector3(1.35f, 0f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalVariables.Instance.objectiveActualTarget != actualQuant && objectiveQuant > 0){
            actualQuant = GlobalVariables.Instance.objectiveActualTarget;
            if(actualQuant <= objectiveQuant){
                tempMark.x = Mathf.Lerp(1.35f, finalMark.x, ((float)actualQuant / objectiveQuant));
                ObjectiveIndicatorTransform.localPosition = new Vector3(tempMark.x, finalMark.y, finalMark.z);
                anim.Play();
            }
        }
    }
}
