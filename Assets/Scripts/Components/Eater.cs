using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour
{
    public event Action OnStarved = () => { };
    
    [SerializeField] private float maxFullness = 10f;

    [SerializeField] private float currentFullenss;
    
    
    public float hungerSpeed;


    public void Init()
    {
        currentFullenss = maxFullness;
    }

    public string GetDebugString()
    {
        return $"{currentFullenss:F2}";
    }
    

    // Update is called once per frame
    void Update()
    {
        currentFullenss -= hungerSpeed * Time.deltaTime;

        if (currentFullenss <= 0)
        {
            OnStarved();
        }
        
    }
}
