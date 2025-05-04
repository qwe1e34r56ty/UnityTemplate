using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDirector
{
    public EntitySpawner entitySpawner;
    public LayoutBuilder layoutBuilder;
    public LayoutInjector layoutInjector;
    public SceneConverter sceneConverter;
    public ResourceManager resourceManager;
    public SceneDirector(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
        entitySpawner = new(this.resourceManager);
        layoutBuilder = new(this.resourceManager);
        layoutInjector = new();
        sceneConverter = new();
    }
    public void ExecuteSceneCommand(GameContext gameContext)
    {
        Queue<ISceneCommand> sceneCommandQueue = gameContext.SceneCommandQueue;
        while(sceneCommandQueue.Count > 0)
        {
            var command = sceneCommandQueue.Dequeue();
            command.Execute(gameContext, this);
        }
    }
}
