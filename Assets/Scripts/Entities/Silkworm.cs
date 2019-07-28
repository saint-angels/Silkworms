using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silkworm : EntityBase
{
    public override void Init()
    {
        base.Init();
        
        GetComponent<Eater>().Init(this);
    }

    public override string GetDebugEntityInfo()
    {
        return "";
    }


    void Update()
    {
        
    }

    public override void Die()
    {
        base.Die();
        
        
    }
}
