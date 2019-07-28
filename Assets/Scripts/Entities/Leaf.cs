using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : EntityBase
{
    public override void Init()
    {
        base.Init();
        
        GetComponent<Food>().Init(this);
    }

    private void Awake()
    {
    }

    public override void Die()
    {
        base.Die();
    }
}
