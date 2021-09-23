using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Stunt : MonoBehaviour
{
    public List<int> comboKeys;
    public string stuntName;
    public bool groundStunt;
    public int stuntValue;
    public bool compare(List<int> candidate){
        if(candidate.Count != comboKeys.Count)return false;
        else return Enumerable.SequenceEqual(candidate, comboKeys);
    }
}