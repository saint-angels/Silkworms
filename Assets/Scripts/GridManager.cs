using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum EntityType
{    
    WORM,
    LEAF,
    MOTH
}

public class GridManager : MonoBehaviour
{
    [SerializeField] private EntityBase wormPrefab;
    [SerializeField] private EntityBase leafPrefab;
    [SerializeField] private EntityBase mothPrefab;
    
    [SerializeField] private GameObject gridBackgroundCell;
    
    public EntityBase[, ] Grid { get; private set; }

    
    public const int gridWidth = 5;
    public const int gridHeight = 5;
    public const float cellWidth = 1f;
    public const float cellHeight = 1f;


    private EntityType[] entitiesToSpawn = new EntityType[] { EntityType.LEAF, EntityType.WORM };
     
    private Dictionary<Direction, Vector2Int> directionVectors = new Dictionary<Direction, Vector2Int>()
    {
        { Direction.UP, Vector2Int.up},
        { Direction.DOWN, Vector2Int.down},
        { Direction.LEFT, Vector2Int.left},
        { Direction.RIGHT, Vector2Int.right},
    };
    
    public void Init()
    {
        Grid = new EntityBase[gridWidth,gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var newBgCell = Instantiate(gridBackgroundCell, IndecesToPosition(x, y), Quaternion.identity);
                newBgCell.transform.SetParent(gameObject.transform);
            }
        }
        
        Root.UIManager.SetHUDForGridCells();
        
        Root.PlayerInput.OnDirectionPressed += OnDirectionPressed;
        Root.PlayerInput.OnResetPressed += ResetGrid;
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
            case EntityType.MOTH:
                newEntity = ObjectPool.Spawn(mothPrefab);
                break;
        }
        
        if (IsPositionValid(x, y) && Grid[x,y] == null)
        {
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
            Grid[entity.X, entity.Y] = null;    
        }
        
        entity.X = newX;
        entity.Y = newY;

        Grid[newX, newY] = entity;

        entity.transform.position = IndecesToPosition(newX, newY);
    }

    private void OnEntityDeath(EntityBase deadEntity)
    {
        Grid[deadEntity.X, deadEntity.Y] = null;
        deadEntity.OnDeath -= OnEntityDeath; 
    }

    private void IterateOverGrid(Direction dir, Action<int,int, EntityBase> action)
    {
        switch (dir)
        {
            case Direction.DOWN:
            case Direction.LEFT:
                for (int x = 0; x < gridWidth; x++)
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        action(x, y, Grid[x,y]);
                    }
                }
                break;
            case Direction.UP:
                for (int x = 0; x < gridWidth; x++)
                {
                    for (int y = gridHeight - 1; y >= 0; y--)
                    {
                        action(x, y, Grid[x,y]);
                    }
                }
                break;
            case Direction.RIGHT:
                for (int x = gridWidth - 1; x >= 0; x--)
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        action(x, y, Grid[x,y]);
                    }
                }
                break;
        }
    }

    private bool IsPositionValid(int x, int y)
    {
        return 0 <= x && x < Grid.GetLength(0) && 0 <= y && y < Grid.GetLength(1);
    }

    public Vector3 IndecesToPosition(int x, int y)
    {
        return new Vector3(x * cellWidth, y * cellHeight, 0);
    }

    private void OnDirectionPressed(Direction direction)
    {
        bool anyEntityShifted = true;

        Vector2Int directionVector = directionVectors[direction];

        IterateOverGrid(direction, (x, y, e) =>
        {
            Vector2Int shiftedIndeces = new Vector2Int(x + directionVector.x, y + directionVector.y);

            bool positionValid = IsPositionValid(shiftedIndeces.x, shiftedIndeces.y);
            EntityBase shiftTargetEntity = positionValid ? Grid[shiftedIndeces.x, shiftedIndeces.y] : null;
            
            bool shiftSpaceEmpty = positionValid &&
                                   shiftTargetEntity == null; 
            if (e != null && shiftSpaceEmpty && e.movedThisTurn == false)
            {
                anyEntityShifted = true;
                MoveEntityToIndeces(e, shiftedIndeces.x, shiftedIndeces.y, true);
                e.movedThisTurn = true;
            }
            else if(e != null && positionValid && shiftTargetEntity != null)
            {
                InteractionResult result = InteractionSystem.Handle(e, shiftTargetEntity);


                switch (result)
                {
                    case InteractionResult.NONE:
                        InteractionResult result2 = InteractionSystem.Handle(shiftTargetEntity, e);
                        switch (result2)
                        {
                            case InteractionResult.TARGET_DEATH:
                                e.Die();
                                break;
                            case InteractionResult.ACTOR_DEATH:
                                shiftTargetEntity.Die();
                                break;
                        }
                        break;
                    case InteractionResult.TARGET_DEATH:
                        shiftTargetEntity.Die();
                        break;
                    case InteractionResult.ACTOR_DEATH:
                        e.Die();
                        break;
                }
            }
        });

        IterateOverGrid(direction,(x, y, e) =>
        {
            if (e != null)
            {
                e.movedThisTurn = false;
            }
        });

        if (anyEntityShifted)
        {
            //Get cell for spawn
            List<Vector2Int> emptyEdgeCells = GetEdgeCellsForDirection(direction);

            if (emptyEdgeCells.Count == 0)
            {
                ResetGrid();
            }
            else
            {
                Vector2Int emptyCell = emptyEdgeCells[UnityEngine.Random.Range(0, emptyEdgeCells.Count)];

                EntityType randomEntityType = entitiesToSpawn[UnityEngine.Random.Range(0, entitiesToSpawn.Length)];
                CreateNewEntity(randomEntityType, emptyCell.x, emptyCell.y);
            }

        }
    }


    private void ResetGrid()
    {
        IterateOverGrid(Direction.LEFT, (x, y, e) =>
        {
            if (e != null)
            {
                e.Die();
            }   
        });
    }

    private List<Vector2Int> GetEdgeCellsForDirection(Direction direction)
    {
        var emptyCells = new List<Vector2Int>();
        
        switch (direction)
        {
            case Direction.UP:

                for (int x = 0; x < gridWidth; x++)
                {
                    if (Grid[x, 0] == null) emptyCells.Add(new Vector2Int(x, 0));
                }
                break;
            case Direction.DOWN:
                for (int x = 0; x < gridWidth; x++)
                {
                    if (Grid[x, gridHeight - 1] == null) emptyCells.Add(new Vector2Int(x, gridHeight - 1));
                }
                break;
            case Direction.LEFT:
                for (int y = 0; y < gridHeight; y++)
                {
                    if (Grid[gridWidth - 1, y] == null) emptyCells.Add(new Vector2Int(gridWidth - 1, y));
                }
                break;
            case Direction.RIGHT:
                for (int y = 0; y < gridHeight; y++)
                {
                    if (Grid[0, y] == null) emptyCells.Add(new Vector2Int(0, y));
                }
                break;
        }

        return emptyCells;
    }
    
    private EntityType GetRandomEntityType()
    {
        Array values = Enum.GetValues(typeof(EntityType));
        Random random = new Random();
        return (EntityType)values.GetValue(random.Next(values.Length));   
    }
}
