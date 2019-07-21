using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{    
    WORM,
    LEAF
}

public class EntityTracker : MonoBehaviour
{

    [SerializeField] private Silkworm wormPrefab = null;
    [SerializeField] private Leaf leafPrefab = null;

    private Dictionary<EntityBase, Food> entityFoods = new Dictionary<EntityBase, Food>();
    
    public void Init()
    {
        
    }

    public EntityBase SpawnEntity(EntityType entityType)
    {
        EntityBase newEntity = null;
        
        switch (entityType)
        {
            case EntityType.WORM:
                newEntity = ObjectPool.Spawn(wormPrefab);
                
                
                break;
            case EntityType.LEAF:
                
                newEntity = ObjectPool.Spawn(leafPrefab);
                entityFoods.Add(newEntity, newEntity.GetComponent<Food>());
                
                break;
        }
        
        newEntity.Init();
        Root.UIManager.SetHUDForEntity(newEntity);
        
        newEntity.OnDeath += OnEntityDeath;
        
        return newEntity;
    }

    public IEnumerable<Food> GetAllFood()
    {
        return entityFoods.Values;
    }
    
    private void OnEntityDeath(EntityBase deadEntity)
    {
        switch (deadEntity.EntityType)
        {
            case EntityType.WORM:
                break;
            case EntityType.LEAF:
                entityFoods.Remove(deadEntity);
                break;
        }
    }
}
