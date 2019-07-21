using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManual : MonoBehaviour
{
    [SerializeField] private GameObject prefab = null;


    
    public void Init()
    {
        Root.PlayerInput.OnDirectionPressed += OnDirectionPressed;    
    }

    private void OnDirectionPressed(Vector2 direction)
    {
        print(direction);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
