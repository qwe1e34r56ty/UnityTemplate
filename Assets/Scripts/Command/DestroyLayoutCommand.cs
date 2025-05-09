using UnityEngine;

public class DestroyLayoutCommand : ISceneCommand
{
    public readonly string layoutID;
    public readonly string message;
    public DestroyLayoutCommand(string layoutID, string message = "")
    {
        this.layoutID = layoutID;
        this.message = message;
    }
    public void Execute(GameContext gameContext, 
        SceneDirector director)
	{
        director.layoutBuilder.LayoutDestroy(gameContext,
            layoutID);
        if (message.Length > 0)
        {
            Logger.Log(message);
        }
    }
}
