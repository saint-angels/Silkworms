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

    protected override void Die()
    {
        base.Die();
    }
}
