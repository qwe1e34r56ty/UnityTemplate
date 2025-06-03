using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using static UnityEngine.EventSystems.EventTrigger;

public class HavePolygonColliderAction : IAction
{
    private readonly Dictionary<Entity, (float LastUpdateTime, float Interval)> entityColliderDatas = new();
    private readonly Dictionary<Entity, PolygonCollider2D> polygonColliders = new();
    private readonly Dictionary<Entity, SpriteRenderer> spriteRenderers = new();
    private readonly Dictionary<Entity, List<Vector2>> previousHulls = new();
    private readonly PolygonColliderConvexGenerator convexGenerator = new();

    public HavePolygonColliderAction()
    {

    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        PolygonCollider2D polygonCollider = entity.root.AddComponent<PolygonCollider2D>();
        SpriteRenderer spriteRenderer = entity.root.GetComponent<SpriteRenderer>();
        polygonColliders.Add(entity, polygonCollider);
        spriteRenderers.Add(entity, spriteRenderer);
        previousHulls.Add(entity, null);
        if (entity.TryGetStat<float>(StatID.PolygonColliderUpdateInterval, out float interval))
        {
            entityColliderDatas.Add(entity, (-999f, interval));
        }
    }

    public void Detach(GameContext gameContext, Entity entity)
    {
        if (entity.root != null)
        {
            PolygonCollider2D collider = polygonColliders[entity];
            if (collider != null)
            {
                GameObject.Destroy(collider);
            }
        }
        entityColliderDatas.Remove(entity);
        spriteRenderers.Remove(entity);
        polygonColliders.Remove(entity);
    }

    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTime)
    {
        if (!polygonColliders.ContainsKey(entity))
        {
            return false;
        }
        if (!spriteRenderers.ContainsKey(entity))
        {
            if (!entity.root.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                return false;
            }
            spriteRenderers.Add(entity, spriteRenderer);
        }
        if (entityColliderDatas.TryGetValue(entity, out var data))
        {
            return Time.time - data.LastUpdateTime >= data.Interval;
        }
        return false;
    }

    public void Execute(GameContext gameContext, Entity entity, float deltaTime)
    {
        if (entityColliderDatas.TryGetValue(entity, out var data))
        {
            var now = Time.time;
            List<Vector2> newHull;
            List<Vector2> prevHull = previousHulls[entity];
            if (convexGenerator.GenerateConvexFromAlpha(spriteRenderers[entity], polygonColliders[entity], prevHull, out newHull))
            {
                prevHull = newHull;
            }
            entityColliderDatas[entity] = (now, data.Interval);
        }
    }
}