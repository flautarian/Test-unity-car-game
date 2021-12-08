using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Scroll{
    internal string stuntAssosiated;
    internal bool unlocked;
    internal bool groundStunt;
    internal int stuntType;
    internal string description;
    internal int[] combination;

    public Scroll(string sa, bool u, bool g, int st, string d, int[] c){
        this.stuntAssosiated = sa;
        this.unlocked = u;
        this.groundStunt = g;
        this.stuntType = st;
        this.description = d;
        this.combination = c;
    }

}