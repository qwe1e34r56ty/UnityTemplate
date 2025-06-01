using System.Diagnostics;

public class GoMainButtonAction : IAction
{
    public GoMainButtonAction()
    {

    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        gameContext.onLeftClickHandlers.Add(entity.root, () =>
        {
            gameContext.sceneCommandQueue.Enqueue(new ConvertSceneCommand(SceneID.Main, $"[{entity.root.name}] request convert to [{SceneID.Main}] scene"));
        });
    }

    public void Detach(GameContext gameContext, Entity entity)
    {
        gameContext.onLeftClickHandlers.Remove(entity.root);
    }

    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTIme)
    {
        return false;
    }

    public void Execute(GameContext gameContext, Entity entity, float deltaTIme)
    {
    }
}