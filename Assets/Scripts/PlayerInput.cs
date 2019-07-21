using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public event Action<Vector2> OnDirectionPressed = (direction) => { };
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnDirectionPressed(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnDirectionPressed(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnDirectionPressed(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnDirectionPressed(Vector2.down);
        }
    }
}
