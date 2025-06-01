using System.Diagnostics;

public class UpdateableAction : IAction
{
    public UpdateableAction()
    {

    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        gameContext.updateHandlers.Add(entity);
    }

    public void Detach(GameContext gameContext, Entity entity)
    {
        gameContext.updateHandlers.Remove(entity);
    }

    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTIme)
    {
        return false;
    }

    public void Execute(GameContext gameContext, Entity entity, float deltaTIme)
    {
    }
}