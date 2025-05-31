using UnityEngine;

public class DestroyEntityCommand : ISceneCommand
{
    public readonly string entityID;
    public readonly string message;
    public DestroyEntityCommand(string entityID, string message = "")
    {
        this.entityID = entityID;
        this.message = message;
    }
    public void Execute(GameContext gameContext, 
        SceneDirector director)
	{
        director.entityBuilder.EntityDestroy(gameContext,
            entityID);
        if (message.Length > 0)
        {
            Logger.Log(message);
        }
    }
}
