using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class PlayerInput : MonoBehaviour
{

    public event Action<Direction> OnDirectionPressed = (direction) => { };
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnDirectionPressed(Direction.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnDirectionPressed(Direction.RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnDirectionPressed(Direction.UP);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnDirectionPressed(Direction.DOWN);
        }
    }
}
