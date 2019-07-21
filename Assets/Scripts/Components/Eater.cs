using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour
{
    public event Action OnStarved = () => { };
    
    public float fullness;
    public float hungerSpeed;


    public void Init()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fullness -= hungerSpeed * Time.deltaTime;

        if (fullness <= 0)
        {
            OnStarved();
        }
        
    }
}
