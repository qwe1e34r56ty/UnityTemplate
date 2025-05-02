using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDirector
{
    public EntitySpawner entitySpawner;
    public LayoutBuilder layoutBuilder;
    public SceneDirector(ResourceManager resourceManager)
    {
        layoutBuilder = new(resourceManager);
        entitySpawner = new(resourceManager);
    }
    public void UpdateScene(GameContext gameContext)
    {
        Queue<ISceneCommand> sceneCommandQueue = gameContext.GetSceneCommandQueue();
        while(sceneCommandQueue.Count > 0)
        {
            var command = sceneCommandQueue.Dequeue();
            command.Execute(gameContext, this);
        }
    }
    public void DestroyScene(GameContext gameContext)
    {
        foreach (var layout in gameContext.layouts)
        {
            layoutBuilder.DestroyLayout(gameContext.layouts, layout.Key);
        }
        gameContext.layouts.Clear();
    }
}
