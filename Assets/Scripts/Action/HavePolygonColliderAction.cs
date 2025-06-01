using System.Collections.Generic;
using UnityEngine;

public class HavePolygonColliderAction : IAction
{
    private Dictionary<Entity, (float LastUpdateTime, float Interval)> entityColliderData = new();

    public HavePolygonColliderAction() { }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        entity.root.AddComponent<PolygonCollider2D>();
        if (entity.TryGetStat<float>(StatID.PolygonColliderUpdateInterval, out float interval))
        {
            entityColliderData.Add(entity, (-999f, interval));
        }
    }

    public void Detach(GameContext gameContext, Entity entity)
    {
        if (entity.root != null)
        {
            var collider = entity.root.GetComponent<PolygonCollider2D>();
            if (collider != null)
            {
                GameObject.Destroy(collider);
            }
        }
        entityColliderData.Remove(entity);
    }

    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTime)
    {
        if (entityColliderData.TryGetValue(entity, out var data))
        {
            return Time.time - data.LastUpdateTime >= data.Interval;
        }
        return false;
    }

    public void Execute(GameContext gameContext, Entity entity, float deltaTime)
    {
        if (entityColliderData.TryGetValue(entity, out var data))
        {
            var now = Time.time;
            var collider = entity.root.GetComponent<PolygonCollider2D>();
            if (collider != null)
            {
                GameObject.Destroy(collider);
                entity.root.AddComponent<PolygonCollider2D>();
            }
            entityColliderData[entity] = (now, data.Interval);
        }
    }
}
