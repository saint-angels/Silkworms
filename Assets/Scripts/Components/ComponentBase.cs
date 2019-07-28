using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBase : MonoBehaviour
{
    public EntityBase Owner { get; private set; }

    public virtual void Init(EntityBase owner)
    {
        Owner = owner;
        
    }

}
