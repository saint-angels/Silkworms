using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnerManual : MonoBehaviour
{
    [SerializeField] private EntityType entityType;

    [SerializeField] private Transform pointLeft = null;
    [SerializeField] private Transform pointRight = null;
    [SerializeField] private Transform pointUp = null;
    [SerializeField] private Transform pointDown = null;
    
    public void Init()
    {
        Root.PlayerInput.OnDirectionPressed += OnDirectionPressed;    
    }

    private void OnDirectionPressed(Direction dir)
    {
        Vector3 spawnPosition = Vector3.zero;


        switch (dir)
        {
            case Direction.UP:
                spawnPosition = pointUp.position;
                break;
            case Direction.DOWN:
                spawnPosition = pointDown.position;
                break;
            case Direction.LEFT:
                spawnPosition = pointLeft.position;
                break;
            case Direction.RIGHT:
                spawnPosition = pointRight.position;
                break;
        }
        
        EntityBase entityBase = Root.EntityTracker.SpawnEntity(GetRandomEntityType());

        var circle = UnityEngine.Random.insideUnitCircle;
        Vector3 randomCircle3 = new Vector3(circle.x, circle.y, 0);
        entityBase.transform.position = spawnPosition + randomCircle3 * 5f;
    }

    private EntityType GetRandomEntityType()
    {
        Array values = Enum.GetValues(typeof(EntityType));
        Random random = new Random();
        return (EntityType)values.GetValue(random.Next(values.Length));   
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(pointLeft.position, Vector3.one);

        if (pointRight)
        {
            Gizmos.DrawWireCube(pointRight.position, Vector3.one);    
        }

        if (pointUp)
        {
            Gizmos.DrawWireCube(pointUp.position, Vector3.one);
        }
        if (pointDown)
        {
            Gizmos.DrawWireCube(pointDown.position, Vector3.one);    
        }
        
    }
}
