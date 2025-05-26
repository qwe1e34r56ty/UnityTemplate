using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class EntityBuilder
{
    private ComponentBuilder componentBuilder;
    private ResourceManager resourceManager;
    public EntityBuilder(ResourceManager resourceManager)
    {
        this.componentBuilder = new(resourceManager);
        this.resourceManager = resourceManager;
    }

    public GameObject EntityBuild(GameContext gameContext,
        string entityID, 
        Vector3? offsetPosition = null, 
        Vector3? offsetRotation = null, 
        Vector3? offsetScale = null,
        int? offsetSortingOrder = null)
    {
        if (gameContext.entityComponentMap.ContainsKey(entityID))
        {
            return null;
        }
        GameObject entityRoot = new GameObject(entityID);
        entityRoot.transform.position = offsetPosition ?? Vector3.zero;
        entityRoot.transform.localScale = offsetScale ?? Vector3.one;
        entityRoot.transform.rotation = offsetRotation.HasValue ? Quaternion.Euler(offsetRotation.Value) : Quaternion.identity;
        gameContext.entityRootMap.Add(entityID, entityRoot);

        Dictionary<string, GameObject> entity = new();
        gameContext.entityComponentMap.Add(entityID, entity);
        foreach (ComponentData componentData in gameContext.entityDataMap[entityID].componentDataArr)
        {
            GameObject component = componentBuilder.ComponentBuild(gameContext, entityID, componentData, offsetSortingOrder: offsetSortingOrder);
            if(component != null)
            {
                component.transform.SetParent(entityRoot.transform, false);
            }
        }
        return entityRoot;
    }

    public void EntityDestroy(GameContext gameContext,
        string entityID)
    {
        if (gameContext.entityComponentMap.ContainsKey(entityID))
        {
            foreach (var pair in gameContext.entityComponentMap[entityID].ToList())
            {
                componentBuilder.ComponentDestroy(gameContext, entityID, pair.Key);
            }
            gameContext.entityComponentMap.Remove(entityID);
        }
        gameContext.entityRootMap.Remove(entityID);
    }
}
