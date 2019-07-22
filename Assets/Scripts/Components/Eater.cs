using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour
{
    public event Action OnStarved = () => { };
    
    [SerializeField] private float maxFullness = 10f;

    [SerializeField] private float currentFullenss;
    [SerializeField] private float biteSize;
    
    
    public float hungerSpeed;


    public void Init()
    {
        currentFullenss = maxFullness;
    }

    public string GetDebugString()
    {
        return $"{currentFullenss:F2}";
    }
    
    
    void Update()
    {
        var availableFood = Root.EntityTracker.GetAllFood();
        for (int i = availableFood.Count - 1; i >= 0; i--)
        {
            Food foodEntity = availableFood[i];

            float distance = Vector3.Distance(foodEntity.transform.position, transform.position);

            if (distance < 2f)
            {
                currentFullenss += foodEntity.Bite(biteSize * Time.deltaTime);
            }
        }

        currentFullenss -= hungerSpeed * Time.deltaTime;

        if (currentFullenss <= 0)
        {
            OnStarved();
        }
        
    }
}
