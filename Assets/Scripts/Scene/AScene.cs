using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public abstract class AScene
{
    public readonly string sceneID;
    public AScene(string sceneID)
    {
        this.sceneID = sceneID;
    }
    public virtual void Build(GameContext gameContext)
    {
        if(!gameContext.sceneLayoutBindingMap.TryGetValue(sceneID, out var layoutIDs)){
            Debug.LogWarning($"Layout Binding not found for scene {sceneID}");
            return;
        }
        Queue<ISceneCommand> sceneCommandQueue = gameContext.SceneCommandQueue;
        foreach(string layoutID in layoutIDs)
        {
            sceneCommandQueue.Enqueue(new BuildLayoutCommand(layoutID, $"{sceneID} Scene {layoutID} layout build request"));
            sceneCommandQueue.Enqueue(new InjectLayoutCommand(layoutID, $"{sceneID} Scene {layoutID} layout inject request"));
        }
    }
    public abstract void Update();
    public virtual void Destroy(GameContext gameContext)
    {
        if (!gameContext.sceneLayoutBindingMap.TryGetValue(sceneID, out var layoutIDs))
        {
            Debug.LogWarning($"Layout Binding not found for scene {sceneID}");
            return;
        }
        Queue<ISceneCommand> sceneCommandQueue = gameContext.SceneCommandQueue;
        foreach (string layoutID in layoutIDs)
        {
            sceneCommandQueue.Enqueue(new DestroyLayoutCommand(layoutID, $"{sceneID} Scene {layoutID} layout destroy request"));
        }
    }
}