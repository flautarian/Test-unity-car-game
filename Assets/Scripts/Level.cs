using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Level{
    internal bool done = false;
    internal int previousLevel;

    public Level(int p){
        this.done = false;
        this.previousLevel = p;
    }
}