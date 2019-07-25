using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hudContainer = null;
    [SerializeField] private HUDBase hudPrefab = null;
    
    [SerializeField] private EntityLine eatLinePrefab = null;
    
    
    private AnimationConfig animationCfg;
    
    private Dictionary<EntityBase, HUDBase> entityHUDS = new Dictionary<EntityBase, HUDBase>();

    private HUDBase[,] gridCellHUDS;
    
    public void Init()
    {
        animationCfg = Root.ConfigManager.Animation;
    }
    
    public void SetHUDForGridCells()
    {
        gridCellHUDS = new HUDBase[GridManager.gridWidth, GridManager.gridHeight];
        for (int x = 0; x < GridManager.gridWidth; x++)
        {
            for (int y = 0; y < GridManager.gridHeight; y++)
            {
                HUDBase newHud = ObjectPool.Spawn(hudPrefab, Vector3.zero, Quaternion.identity, hudContainer);
                gridCellHUDS[x, y] = newHud;
            }
        }
    }

    public void SetHUDForEntity(EntityBase entityBase)
    {
        HUDBase newHud = ObjectPool.Spawn(hudPrefab, Vector3.zero, Quaternion.identity, hudContainer);
        
        entityHUDS.Add(entityBase, newHud);

        entityBase.OnDeath += EntityBaseOnDeath;
    }

    private void EntityBaseOnDeath(EntityBase entity)
    {
        ObjectPool.Despawn(entityHUDS[entity]);
        entityHUDS.Remove(entity);
        
        
        entity.OnDeath -= EntityBaseOnDeath;
    }

    private void LateUpdate()
    {
        foreach (var entityHUDPair in entityHUDS)
        {
            var entity = entityHUDPair.Key;
            var hud = entityHUDPair.Value;
            
            hud.SetText(entity.GetDebugEntityInfo());
            
            Vector2 screenPoint = Root.CameraController.WorldToScreenPoint(entity.HUDPoint);
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hudContainer, screenPoint, null, out localPoint))
            {
                hud.transform.localPosition = localPoint;
            }
        }

        if (gridCellHUDS != null)
        {
            for (int x = 0; x < GridManager.gridWidth; x++)
            {
                for (int y = 0; y < GridManager.gridHeight; y++)
                {
                    var hud = gridCellHUDS[x,y];

                    Vector3 worldPosition = Root.GridManager.IndecesToPosition(x, y) + new Vector3(GridManager.cellWidth / 2f, GridManager.cellHeight/ 2f, 0);
                    Vector2 screenPoint = Root.CameraController.WorldToScreenPoint(worldPosition);
                    Vector2 localPoint;
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hudContainer, screenPoint, null, out localPoint))
                    {
                        hud.transform.localPosition = localPoint;
                    }

                    var entityBase = Root.GridManager.Grid[x, y];

                    if (entityBase == null)
                    {
                        hud.SetText("null");
                    }
                    else
                    {
                        hud.SetText(entityBase.EntityType.ToString());
                    }
                }
            }
        }
        
    }
}
