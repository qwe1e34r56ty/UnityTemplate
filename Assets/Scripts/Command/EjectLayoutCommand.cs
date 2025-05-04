using UnityEngine;

public class EjectLayoutCommand : ISceneCommand
{
    public readonly string layoutID;
    public readonly string message;
    public EjectLayoutCommand(string layoutID, string message = "")
    {
        this.layoutID = layoutID;
        this.message = message;
    }
    public void Execute(GameContext gameContext,
        SceneDirector director)
    {
        director.layoutInjector.Eject(gameContext,
            gameContext.layouts[layoutID],
            layoutID);
        if (message.Length > 0)
        {
            Debug.Log(message);
        }
    }
}
