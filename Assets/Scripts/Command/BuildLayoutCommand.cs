using UnityEngine;

public class BuildLayoutCommand : ISceneCommand
{
    public readonly string layoutID;
    public readonly string message;
    public BuildLayoutCommand(string layoutID, string message = "")
    {
        this.layoutID = layoutID;
        this.message = message;
    }
    public void Execute(GameContext gameContext, SceneDirector builder)
    {
        builder.layoutBuilder.BuildLayout(gameContext.layouts, gameContext.layoutDataMap, layoutID);
        if (message.Length > 0)
        {
            Debug.Log(message);
        }
    }
}
