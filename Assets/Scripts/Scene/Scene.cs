using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Scene
{
    public string sceneID;
    public void Build(GameContext gameContext, string sceneID)
    {
        this.sceneID = sceneID;
        if(!gameContext.sceneDataMap.TryGetValue(sceneID, out var sceneData)){
            Logger.LogWarning($"SceneData not found for scene {sceneID}");
            return;
        }
        Queue<ISceneCommand> sceneCommandQueue = gameContext.sceneCommandQueue;
        foreach(EntityTransformData entityTransform in sceneData.entityTransformDataArr)
        {
            sceneCommandQueue.Enqueue(
                new BuildEntityCommand(entityTransform.id, 
                $"{sceneID} Scene {entityTransform.id} entity build request", 
                entityTransform.offsetPosition,
                entityTransform.offsetRotation,
                entityTransform.offsetScale,
                entityTransform.offsetSortingOrder)
                );
        }
    }

    public void Clear(GameContext gameContext)
    {
        Queue<ISceneCommand> sceneCommandQueue = gameContext.sceneCommandQueue;
        foreach (var pair in gameContext.entities)
        {
            sceneCommandQueue.Enqueue(new DestroyEntityCommand(pair.Key, $"[{sceneID} Scene] {pair.Key.name} entity destroy request"));
        }
    }
}