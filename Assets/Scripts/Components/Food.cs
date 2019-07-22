using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public event Action OnEmpty = () => { };

    [SerializeField] private float maxAmount;
    
    private float foodAmount;
    

    public void Init()
    {
        foodAmount = maxAmount;
    }

    public float Bite(float biteSize)
    {
        float bite = Mathf.Min(biteSize, foodAmount);
        foodAmount -= bite;

        if (Mathf.Approximately(foodAmount,0))
        {
            OnEmpty();
        }

        return bite;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
