using UnityEngine;

public class ConvertSceneCommand : ISceneCommand
{
    public readonly string sceneID;
    public readonly string message;
    public ConvertSceneCommand(string sceneID, string message = "")
    {
        this.sceneID = sceneID;
        this.message = message;
    }
    public void Execute(GameContext gameContext,
        SceneDirector director)
    {
        director.sceneConverter.ConvertScene(gameContext, sceneID);
        if (message.Length > 0)
        {
            Debug.Log(message);
        }
    }
}