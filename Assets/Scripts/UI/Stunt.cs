using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Stunt : MonoBehaviour
{
    public List<int> comboKeys;
    public string stuntName;
    public bool groundStunt;

    // cost if is a def or atk stunt, or quant of stunt points earned if are normal stunt
    public int units;
    public StuntType stuntType;

    public string chunkName;

    [SerializeField]
    private AnimationClip anim;

    private void Start() {
    }
    public bool compare(List<int> candidate){
        if(candidate.Count != comboKeys.Count)return false;
        else return Enumerable.SequenceEqual(candidate, comboKeys);
    }

    public AnimationClip GetAnimation(){
        return anim;
    }
}
