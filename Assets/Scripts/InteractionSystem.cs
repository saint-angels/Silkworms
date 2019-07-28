using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum InteractionResult
{
    NONE,
    SUCCESS,
    TARGET_DEATH,
    ACTOR_DEATH
}

public static class InteractionSystem
{
    public static InteractionResult Handle(EntityBase entity1, EntityBase entity2)
    {
        var eater1 = entity1.GetComponent<Eater>();
        var food2 = entity2.GetComponent<Food>();

        if (eater1 != null && food2 != null)
        {
            if (TryEat(eater1, food2))
            {
                return InteractionResult.TARGET_DEATH;
            }
            else
            {
                return InteractionResult.SUCCESS;
            }
        }
        else
        {
            return InteractionResult.NONE;
        }
        
        

        return InteractionResult.NONE;
    }

    private static bool TryEat(Eater eater, Food food)
    {
        return true;
    }
}
