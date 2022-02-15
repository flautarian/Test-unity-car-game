using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public enum MovementType{
        TOWARDS, LERP
    }
    [SerializeField]
    private List<Vector3> positions;

    [SerializeField]
    private int actualPosition = 0;
    
    [SerializeField]
    private bool looping = false;

    [SerializeField]
    private float velocity = 0f;

    [SerializeField]
    private MovementType movementType;

    // Update is called once per frame
    void Update()
    {
        if(positions.Count >= actualPosition && transform.localPosition != positions[actualPosition]){
            switch(movementType){
                case MovementType.TOWARDS:
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, positions[actualPosition], Time.deltaTime * velocity); 
                break;
                case MovementType.LERP:
                    transform.localPosition = Vector3.Lerp(transform.localPosition, positions[actualPosition], Time.deltaTime * velocity); 
                break;
            }
        }
        else if(looping)
            MoveToNextPosition();
    }

    public void MoveToNextPosition(){
        if(actualPosition < positions.Count -1)
            actualPosition++;
        else actualPosition = 0;
    }

    public void MoveToLastPosition(){
        if(actualPosition > 1)
            actualPosition--;
    }

    public void MoveToFirstPosition(){
        actualPosition = 0;
    }

    public void MoveToPosition(int val){
        if(val < positions.Count)
            actualPosition = val;
    }

    public int GetActualPosition(){
        return actualPosition;
    }
}
