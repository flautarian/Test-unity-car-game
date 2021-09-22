using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Stunt : MonoBehaviour
{
    public string[] comboKeys;

    public string comboName;
    public bool compare(string[] candidate){
        if(candidate.Length != comboKeys.Length)return false;
        else return Enumerable.SequenceEqual(candidate, comboKeys);
    }
}
