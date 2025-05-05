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
        if (gameContext.layouts.TryGetValue(layoutID, out var layout))
        {
            director.layoutInjector.Eject(gameContext,
                layout,
                layoutID);
        }
        if (message.Length > 0)
        {
            Debug.Log(message);
        }
    }
}
