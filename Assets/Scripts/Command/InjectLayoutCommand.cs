using UnityEngine;

public class InjectLayoutCommand : ISceneCommand
{
    public readonly string layoutID;
    public readonly string message;
    public InjectLayoutCommand(string layoutID, string message = "")
    {
        this.layoutID = layoutID;
        this.message = message;
    }
    public void Execute(GameContext gameContext,
        SceneDirector director)
    {
        director.layoutInjector.Inject(gameContext,
            layoutID);
        if (message.Length > 0)
        {
            Logger.Log(message);
        }
    }
}
