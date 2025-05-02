using UnityEngine;

public class BuildLayoutCommand : ISceneCommand
{
    public readonly string layoutID;
    public BuildLayoutCommand(string layoutID)
    {
        this.layoutID = layoutID;
    }
    public void Execute(GameContext gameContext, SceneDirector builder)
    {
        builder.layoutBuilder.BuildLayout(gameContext.layouts, gameContext.layoutDataMap, layoutID);
    }
}
