using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : ComponentBase
{
    public event Action OnEmpty = () => { };

    [SerializeField] private int maxAmount;
    
    private int foodAmount;
    

    public override void Init(EntityBase owner)
    {
        base.Init(owner);

        foodAmount = maxAmount;
    }

    public int Bite(int biteSize)
    {
        int bite = Mathf.Min(biteSize, foodAmount);
        foodAmount -= bite;

        if (foodAmount == 0)
        {
            OnEmpty();
        }

        return bite;
    }
}
