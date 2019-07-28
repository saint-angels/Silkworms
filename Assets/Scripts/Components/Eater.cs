using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : ComponentBase
{

    public event Action OnStarved = () => { };
    
    public int maxFullness = 3;

    public int currentFullenss;
    public int biteSize;


    public override void Init(EntityBase owner)
    {
        base.Init(owner);
        currentFullenss = maxFullness;
    }

    
    

}
