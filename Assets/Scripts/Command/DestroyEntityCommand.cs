using UnityEngine;

public class DestroyEntityCommand : ISceneCommand
{
    public readonly GameObject gameObject;
    public readonly string message;
    public DestroyEntityCommand(GameObject gameObject, string message = "")
    {
        this.gameObject = gameObject;
        this.message = message;
    }
    public void Execute(GameContext gameContext, 
        SceneDirector director)
	{
        director.entityBuilder.EntityDestroy(gameContext,
            gameObject);
        if (message.Length > 0)
        {
            Logger.Log(message);
        }
    }
}
