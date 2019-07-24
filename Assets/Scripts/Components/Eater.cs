using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour
{
    private enum EaterState
    {
        IDLE,
        APPROACHING_FOOD,
        EATING
    }
    
    public event Action OnStarved = () => { };
    
    public Food TargetFood { get; private set; }
    
    [SerializeField] private float maxFullness = 10f;

    [SerializeField] private float currentFullenss;
    [SerializeField] private float biteSize;
    [SerializeField] private float eatDistance = 1f;
    
    public float hungerSpeed;


    private EaterState state;

    public void Init()
    {
        currentFullenss = maxFullness;
        state = EaterState.IDLE;
    }

    public string GetDebugString()
    {
        return $"{currentFullenss:F2}";
    }
    
    
    void Update()
    {
        if (TargetFood == null)
        {
            state = EaterState.IDLE;
        }
        
        
        switch (state)
        {
            case EaterState.IDLE:
                IdleBehaviour();
                break;
            case EaterState.APPROACHING_FOOD:
                AprroachingFoodBehaviour();
                break;
            case EaterState.EATING:
                EatingBehaviour();
                break;
        }

        currentFullenss -= hungerSpeed * Time.deltaTime;

        if (currentFullenss <= 0)
        {
            OnStarved();
        }
        
    }

    private void EatingBehaviour()
    {
        float distance = Vector3.Distance(TargetFood.transform.position, transform.position);

        if (distance <= eatDistance)
        {
            currentFullenss += TargetFood.Bite(biteSize * Time.deltaTime);
        }
        else
        {
            state = EaterState.APPROACHING_FOOD;
        }
    }

    private void AprroachingFoodBehaviour()
    {
        float distance = Vector3.Distance(TargetFood.transform.position, transform.position);

        if (distance <= eatDistance)
        {
            state = EaterState.EATING;
        }
    }

    private void IdleBehaviour()
    {
//        Food closestFood = null;
//        float closestFoodDistance = float.MaxValue;
//        
//        var availableFood = Root.EntityTracker.GetAllFood();
//        for (int i = availableFood.Count - 1; i >= 0; i--)
//        {
//            Food foodEntity = availableFood[i];
//
//            float distance = Vector3.Distance(foodEntity.transform.position, transform.position);
//
//
//            if (distance < closestFoodDistance)
//            {
//                TargetFood = foodEntity;
//            }
//        }
//
//        if (TargetFood != null)
//        {
//            state = EaterState.APPROACHING_FOOD;            
//        }
    }
}
