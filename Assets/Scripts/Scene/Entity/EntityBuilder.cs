using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class EntityBuilder
{
    private ResourceManager resourceManager;
    public EntityBuilder(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
    }

    public GameObject EntityBuild(GameContext gameContext,
        string entityID, 
        Vector3? offsetPosition = null, 
        Vector3? offsetRotation = null, 
        Vector3? offsetScale = null,
        int offsetSortingOrder = 0)
    {
        if (!gameContext.entityDataMap.TryGetValue(entityID, out EntityData entityData)){
            Logger.Log($"[EntityBuilder] EntityData not found : [id : {entityID}]");
            return null;
        }
        Entity entity = new Entity(gameContext, resourceManager, entityData, offsetPosition, offsetRotation, offsetScale, offsetSortingOrder);
        return entity.root;
    }

    public void EntityDestroy(GameContext gameContext, GameObject gameObject)
    {
        if (gameContext.entities.TryGetValue(gameObject, out Entity entity))
        {
            entity.Destroy(gameContext);
        }
    }
}
