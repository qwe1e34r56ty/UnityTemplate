using System.Diagnostics;

public class LoadSceneButtonAction : IAction
{
    public LoadSceneButtonAction()
    {

    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        if (entity.TryGetStat<string>(StatID.LoadScene, out string nextScene))
        {
            gameContext.onLeftClickHandlers.Add(entity.root, () =>
            {
                gameContext.sceneCommandQueue.Enqueue(new ConvertSceneCommand(nextScene, $"[{entity.root.name}] request convert to [{nextScene}] scene"));
            });
        }
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