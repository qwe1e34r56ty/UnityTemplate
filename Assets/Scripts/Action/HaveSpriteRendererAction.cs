using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class HaveSpriteRendererAction : IAction
{
    private readonly Dictionary<Entity, SpriteRenderer> spriteRenderers = new();
    public HaveSpriteRendererAction()
    {
    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        SpriteRenderer spriteRenderer = entity.root.AddComponent<SpriteRenderer>();
        spriteRenderers.Add(entity, spriteRenderer);
        if (entity.TryGetStat<int>(StatID.OffsetSortingOrder, out int offsetSortingOrder))
        {
            spriteRenderer.sortingOrder += offsetSortingOrder;
        }
    }

    public void Detach(GameContext gameContext, Entity entity)
    {
        if (spriteRenderers.TryGetValue(entity, out SpriteRenderer spriteRenderer))
        {
            spriteRenderers.Remove(entity);
            if (spriteRenderer != null)
            {
                GameObject.Destroy(spriteRenderer);
            }
        }
    }

    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTIme)
    {
        return false;
    }

    public void Execute(GameContext gameContext, Entity entity, float deltaTIme)
    {
    }
}