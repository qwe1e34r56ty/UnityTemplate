using UnityEngine;

public class BuildEntityCommand : ISceneCommand
{
    public readonly string entityID;
    public readonly string message;
    public readonly Vector3? position;
    public readonly Vector3? rotation;
    public readonly Vector3? scale;
    public readonly int sortingOrder;

    public BuildEntityCommand(string entityID, 
        string message = "",
        Vector3? position = null,
        Vector3? rotation = null,
        Vector3? scale = null,
        int sortingOrder = 0)
    {
        this.entityID = entityID;
        this.message = message;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        this.sortingOrder = sortingOrder;
    }
    public void Execute(GameContext gameContext, 
        SceneDirector director)
    {
        director.entityBuilder.EntityBuild(gameContext,
            entityID,
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
