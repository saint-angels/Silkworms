using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum EntityType
{    
    WORM,
    LEAF
}

public class GridManager : MonoBehaviour
{
    [SerializeField] private EntityBase wormPrefab;
    [SerializeField] private EntityBase leafPrefab;
    
    private EntityBase[, ] grid;

    private int gridWidth = 5;
    private int gridHeight = 5;
    
    private Dictionary<Direction, Vector2Int> directionVectors = new Dictionary<Direction, Vector2Int>()
    {
        { Direction.UP, Vector2Int.up},
        { Direction.DOWN, Vector2Int.down},
        { Direction.LEFT, Vector2Int.left},
        { Direction.RIGHT, Vector2Int.right},
    };
    
    public void Init()
    {
        grid = new EntityBase[gridWidth,gridHeight];
        
        Root.PlayerInput.OnDirectionPressed += OnDirectionPressed;
    }

    public void CreateNewEntity(EntityType entityType, int x, int y)
    {
        EntityBase newEntity = null;
        switch (entityType)
        {
            case EntityType.WORM:
                newEntity = ObjectPool.Spawn(wormPrefab);
                break;
            case EntityType.LEAF:
                newEntity = ObjectPool.Spawn(leafPrefab);
                break;
        }
        
        if (IsPositionValid(x, y) && grid[x,y] == null)
        {
            grid[x, y] = newEntity;
            newEntity.OnDeath += OnEntityDeath; 
            newEntity.Init();
            
            MoveEntityToIndeces(newEntity, x, y, false);
        }
        else
        {
            Debug.LogError($"{x}:{y} is not empty!!!");
        }
    }

    private void MoveEntityToIndeces(EntityBase entity, int newX, int newY, bool clearPrevious)
    {
        if (clearPrevious)
        {
            grid[entity.X, entity.Y] = null;    
        }
        
        entity.X = newX;
        entity.Y = newY;

        entity.transform.position = IndecesToPosition(newX, newY);
    }

    private void OnEntityDeath(EntityBase deadEntity)
    {
        grid[deadEntity.X, deadEntity.Y] = null;
        deadEntity.OnDeath -= OnEntityDeath; 
    }

    private void IterateOverGrid(Action<int,int, EntityBase> action)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                action(x, y, grid[x,y]);
            }
        }
    }

    private bool IsPositionValid(int x, int y)
    {
        return 0 <= x && x < grid.GetLength(0) && 0 <= y && y < grid.GetLength(1);
    }

    private Vector3 IndecesToPosition(int x, int y)
    {
        return new Vector3(x, y, 0);
    }

    private void OnDirectionPressed(Direction direction)
    {
        //Can shift?
        //TODO: Check if shift is possible
        bool anyEntityShifted = true;
        
        //TODO: Process cells according to input direction

        Vector2Int directionVector = directionVectors[direction];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                EntityBase e = grid[x, y];
                Vector2Int shiftIndeces = new Vector2Int(x + directionVector.x, y + directionVector.y);

                bool shiftSpaceEmpty = IsPositionValid(shiftIndeces.x, shiftIndeces.y) &&
                                  grid[shiftIndeces.x, shiftIndeces.y] == null; 
                if (e != null && shiftSpaceEmpty)
                {
                    MoveEntityToIndeces(e, shiftIndeces.x, shiftIndeces.y, true);
                }
            }
        }
        
        
        
        //Get cell for spawn
        List<Vector2Int> emptyEdgeCells = GetEdgeCellsForDirection(direction);

        Vector2Int emptyCell = emptyEdgeCells[UnityEngine.Random.Range(0, emptyEdgeCells.Count)];
        
        CreateNewEntity(EntityType.WORM, emptyCell.x, emptyCell.y);
    }

    private List<Vector2Int> GetEdgeCellsForDirection(Direction direction)
    {
        var emptyCells = new List<Vector2Int>();
        
        switch (direction)
        {
            case Direction.UP:

                for (int x = 0; x < gridWidth; x++)
                {
                    if (grid[x, 0] == null) emptyCells.Add(new Vector2Int(x, 0));
                }
                break;
            case Direction.DOWN:
                for (int x = 0; x < gridWidth; x++)
                {
                    if (grid[x, gridHeight - 1] == null) emptyCells.Add(new Vector2Int(x, gridHeight - 1));
                }
                break;
            case Direction.LEFT:
                for (int y = 0; y < gridHeight; y++)
                {
                    if (grid[gridWidth - 1, y] == null) emptyCells.Add(new Vector2Int(gridWidth - 1, y));
                }
                break;
            case Direction.RIGHT:
                for (int y = 0; y < gridHeight; y++)
                {
                    if (grid[0, y] == null) emptyCells.Add(new Vector2Int(0, y));
                }
                break;
        }

        return emptyCells;
    }
}
