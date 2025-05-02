using System.Collections.Generic;
using UnityEngine;
public class GameContext
{
    private SaveData? saveData;
    public Queue<ISceneCommand> SceneCommandQueue = new();
    public Dictionary<string, Dictionary<string, GameObject>> layouts = new();
    public Dictionary<string, string> layoutPathMap = new();
    public Dictionary<string, LayoutData> layoutDataMap = new();
    public GameContext(SaveData? saveData)
    {
        this.saveData = saveData;
    }
    public Queue<ISceneCommand> GetSceneCommandQueue()
    {
        return SceneCommandQueue;
    }

    public void ClearLayout()
    {
        layouts.Clear();
    }

    public void RegisterLayout(string layoutID, Dictionary<string, GameObject> layout)
    {
        layouts.Add(layoutID, layout);
    }
}
