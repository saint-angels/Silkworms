using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    public event Action<EntityBase> OnDeath = (entity) => { };
    
    public bool movedThisTurn { get; set; }
    
    public EntityBase InteractingWith { get; set; }
    
    public int X { get; set; }
    public int Y { get; set; }

    public EntityType EntityType;
    
    public Vector3 HUDPoint => transform.position + Vector3.up;

    public virtual void Init()
    {
    }

    public virtual string GetDebugEntityInfo()
    {
        return string.Empty;
    }

    public virtual void Die()
    {
        OnDeath(this);
        
        ObjectPool.Despawn(this);
    }

}
