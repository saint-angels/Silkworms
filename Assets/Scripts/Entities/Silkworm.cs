using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silkworm : EntityBase
{

    private Eater eater;
    
    private void Awake()
    {
        eater = GetComponent<Eater>();
        eater.OnStarved += Die;
    }

    public override string GetDebugEntityInfo()
    {
        return eater.GetDebugString();
    }


    public override void Init()
    {
        base.Init();
        eater.Init();
    }

 
    void Update()
    {
        
    }

    protected override void Die()
    {
        base.Die();
        
        
    }
}
