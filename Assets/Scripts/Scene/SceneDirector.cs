using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDirector
{
    public EntityBuilder entityBuilder;
    public SceneConverter sceneConverter;
    public ResourceManager resourceManager;
    public SceneDirector(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
        entityBuilder = new(this.resourceManager);
        sceneConverter = new();
    }
    public void ExecuteSceneCommand(GameContext gameContext)
    {
        Queue<ISceneCommand> sceneCommandQueue = gameContext.sceneCommandQueue;
        while(sceneCommandQueue.Count > 0)
        {
            var command = sceneCommandQueue.Dequeue();
            command.Execute(gameContext, this);
        }
    }
}
