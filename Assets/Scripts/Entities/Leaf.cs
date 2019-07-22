using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : EntityBase
{
    private Food food;
    
    private void Awake()
    {
        food = GetComponent<Food>();
        food.OnEmpty += Die;
    }
    
    public override void Init()
    {
        base.Init();
        
        food.Init();
    }

    protected override void Die()
    {
        base.Die();
    }
}
