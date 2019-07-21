using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hudContainer = null;
    [SerializeField] private HUDBase hudPrefab = null;
    
    
    private AnimationConfig animationCfg;
    
    private Dictionary<EntityBase, HUDBase> entityHUDS = new Dictionary<EntityBase, HUDBase>();

    public void Init()
    {
        animationCfg = Root.ConfigManager.Animation;

        
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
    }
}
