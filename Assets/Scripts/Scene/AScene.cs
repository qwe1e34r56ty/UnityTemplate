using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public abstract class AScene : IUpdatable
{
    public readonly string id;
    public AScene(string id)
    {
        this.id = id;
    }
    public virtual void Build(GameContext gameContext)
    {
        if(!gameContext.sceneDataMap.TryGetValue(id, out var sceneData)){
            Logger.LogWarning($"SceneData not found for scene {id}");
            return;
        }
        Queue<ISceneCommand> sceneCommandQueue = gameContext.sceneCommandQueue;
        foreach(EntityTransformData entityTransform in sceneData.entityTransformDataArr)
        {
            sceneCommandQueue.Enqueue(
                new BuildEntityCommand(entityTransform.id, 
                $"{id} Scene {entityTransform.id} entity build request", 
                entityTransform.offsetPosition,
                entityTransform.offsetRotation,
                entityTransform.offsetScale,
                entityTransform.offsetSortingOrder)
                );
        }
    }
    public virtual void Update(float deltaTime)
    {

    }

    public virtual void Destroy(GameContext gameContext)
    {
        if (!gameContext.sceneDataMap.TryGetValue(id, out var sceneData))
        {
            Logger.LogWarning($"SceneData not found for scene {id}");
            return;
        }
        Queue<ISceneCommand> sceneCommandQueue = gameContext.sceneCommandQueue;
        foreach (EntityTransformData entityTransform in sceneData.entityTransformDataArr)
        {
            sceneCommandQueue.Enqueue(new DestroyEntityCommand(entityTransform.id, $"{id} Scene {entityTransform.id} entity destroy request"));
        }
    }
}