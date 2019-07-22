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
    private Dictionary<Eater, EntityLine> eaterLines = new Dictionary<Eater, EntityLine>();

    public void Init()
    {
        animationCfg = Root.ConfigManager.Animation;

        
    }

    public void SetHUDForEntity(EntityBase entityBase)
    {
        HUDBase newHud = ObjectPool.Spawn(hudPrefab, Vector3.zero, Quaternion.identity, hudContainer);
        
        entityHUDS.Add(entityBase, newHud);

        if (entityBase.EntityType == EntityType.WORM)
        {
            EntityLine newEaterLine = ObjectPool.Spawn(eatLinePrefab, Vector3.zero, Quaternion.identity);
            eaterLines.Add(entityBase.GetComponent<Eater>(), newEaterLine);
        }
        
        entityBase.OnDeath += EntityBaseOnDeath;
    }

    private void EntityBaseOnDeath(EntityBase entity)
    {
        ObjectPool.Despawn(entityHUDS[entity]);
        entityHUDS.Remove(entity);
        
        if (entity.EntityType == EntityType.WORM)
        {
            var eater = entity.GetComponent<Eater>();
            ObjectPool.Despawn(eaterLines[eater]);
            eaterLines.Remove(eater);
        }
        
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

        foreach (var eaterLineKeyPair in eaterLines)
        {
            var eater = eaterLineKeyPair.Key;
            var line = eaterLineKeyPair.Value;

            bool haveFoodTarget = eater.TargetFood != null;
            
            line.gameObject.SetActive(haveFoodTarget);
            if (haveFoodTarget)
            {
                line.Init(eater.transform.position, eater.TargetFood.transform.position);        
            }
            
            
        }
    }
}
