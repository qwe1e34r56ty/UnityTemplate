using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;

public class ComponentBuilder
{
    private ResourceManager resourceManager;
    public ComponentBuilder(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
    }
    public GameObject ComponentBuild(GameContext gameContext,
        string entityID,
        ComponentData componentData,
        Vector3? offsetPosition = null,
        Vector3? offsetRotation = null,
        Vector3? offsetScale = null,
        int? offsetSortingOrder = null)
    {
        if(!gameContext.entityComponentMap.TryGetValue(entityID, out var entity))
        {
            return null;
        }
        if (entity.ContainsKey(componentData.id))
        {
            return null;
        }
        GameObject gameObject = new GameObject(componentData.id);
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        if(!gameContext.animationDataMap.TryGetValue(componentData.animationID, out var animation))
        {
            Logger.LogWarning($"[ComponentBuilder] Animation not found in animationDataMap {componentData.animationID}");
            return null;
        }

        spriteRenderer.sortingOrder = componentData.sortingOrder + (offsetSortingOrder ?? 0);
        gameObject.transform.position = new Vector3(componentData.x, componentData.y, 0f) + (offsetPosition ?? Vector3.zero);
        gameObject.transform.localScale = offsetScale ?? Vector3.one;
        gameObject.transform.rotation = offsetRotation.HasValue ? Quaternion.Euler(offsetRotation.Value) : Quaternion.identity;

        AnimationPlayer animationPlayer = new();
        gameContext.animationPlayerMap.Add(gameObject, animationPlayer);
        animationPlayer.Play(gameObject, animation);
        gameContext.updateHandlers.Add(animationPlayer);

        if (TagUtility.IsValidTagName(componentData.tagName))
        {
            gameObject.tag = componentData.tagName;
        }
        if (LayerUtility.IsValidLayerName(componentData.layerName))
        {
            gameObject.layer = LayerMask.NameToLayer(componentData.layerName);
        }
        spriteRenderer.sortingOrder += offsetSortingOrder ?? 0;

        entity.Add(componentData.id, gameObject);
        return gameObject;
    }

    public void ComponentDestroy(GameContext gameContext, string entityID, string componentID)
    {
        if (gameContext.entityComponentMap.TryGetValue(entityID, out var entity))
        {
            if (entity.TryGetValue(componentID, out var component)) 
            {
                entity.Remove(componentID);
                if (gameContext.animationPlayerMap.TryGetValue(component, out var animationPlayer))
                {
                    gameContext.updateHandlers.Remove(animationPlayer);
                    gameContext.animationPlayerMap.Remove(component);
                }
                GameObject.Destroy(component);
            }
        }
    }
}