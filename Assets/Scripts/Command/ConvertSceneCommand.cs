using UnityEngine;

public class ConvertSceneCommand : ISceneCommand
{
    public readonly string id;
    public readonly string message;
    public ConvertSceneCommand(string id, string message = "")
    {
        this.id = id;
        this.message = message;
    }
    public void Execute(GameContext gameContext,
        SceneDirector director)
    {
        director.sceneConverter.ConvertScene(gameContext, id);
        if (message.Length > 0)
        {
            Logger.Log(message);
        }
    }
}