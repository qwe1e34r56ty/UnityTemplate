using System.Diagnostics;

public class GoTitleButtonAction : IAction
{
    public GoTitleButtonAction()
    {

    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        gameContext.onLeftClickHandlers.Add(entity.root, () =>
        {
            gameContext.sceneCommandQueue.Enqueue(new ConvertSceneCommand(SceneID.Start, $"[{entity.root.name}] request convert to [{SceneID.Main}] scene"));
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