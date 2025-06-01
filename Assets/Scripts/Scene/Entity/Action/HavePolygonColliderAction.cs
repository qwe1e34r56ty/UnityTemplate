using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class HavePolygonColliderActionAction : IAction
{
    private readonly float interval = 0.1f;
    private Dictionary<Entity, float> lastUpdateTimeMap = new();
    public HavePolygonColliderActionAction()
    {

    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        entity.root.AddComponent<PolygonCollider2D>();
        lastUpdateTimeMap.Add(entity, -999f);
    }

    public void Detach(GameContext gameContext, Entity entity)
    {
        if (entity.root != null)
        {
            var collider = entity.root.GetComponent<PolygonCollider2D>();
            if (collider != null)
                GameObject.Destroy(collider);
        }
        lastUpdateTimeMap.Remove(entity);
    }

    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTIme)
    {
        float lastTime = lastUpdateTimeMap[entity];
        return Time.time - lastTime >= interval;
    }

    public void Execute(GameContext gameContext, Entity entity, float deltaTIme)
    {
        float now = Time.time;
        var collider = entity.root.GetComponent<PolygonCollider2D>();
        if (collider != null)
        {
            GameObject.Destroy(collider);
            entity.root.AddComponent<PolygonCollider2D>();
        }
        lastUpdateTimeMap[entity] = now;
    }
}