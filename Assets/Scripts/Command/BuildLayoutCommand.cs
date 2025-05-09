using UnityEngine;

public class BuildLayoutCommand : ISceneCommand
{
    public readonly string layoutID;
    public readonly string message;
    public readonly Vector3? position;
    public readonly Vector3? rotation;
    public readonly Vector3? scale;
    public readonly int? sortingOrder;

    public BuildLayoutCommand(string layoutID, 
        string message = "",
        Vector3? position = null,
        Vector3? rotation = null,
        Vector3? scale = null,
        int? sortingOrder = null)
    {
        this.layoutID = layoutID;
        this.message = message;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        this.sortingOrder = sortingOrder;
    }
    public void Execute(GameContext gameContext, 
        SceneDirector director)
    {
        director.layoutBuilder.LayoutBuild(gameContext,
            layoutID,
            position,
            rotation,
            scale,
            sortingOrder);
        if (message.Length > 0)
        {
            Logger.Log(message);
        }
    }
}
