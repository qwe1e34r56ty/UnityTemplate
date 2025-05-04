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
        director.layoutBuilder.LayoutDestroy(gameContext.layouts, 
            gameContext.layoutRootMap,
            layoutID);
        if (message.Length > 0)
        {
            Debug.Log(message);
        }
    }
}
