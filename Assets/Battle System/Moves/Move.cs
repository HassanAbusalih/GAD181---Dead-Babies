using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{

    public Moves Base;
    public int powerpoints;

    public Move(Moves mBase)
    {
        Base = mBase;
        powerpoints = mBase.powerpoints;
    }
}
