using System.Diagnostics;

public class EmptyAction : IAction
{
    public EmptyAction()
    {

    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        Logger.Log($"[EmptyAction] Attacehd on [{entity.root.name}]");
    }

    public void Detach(GameContext gameContext, Entity entity)
    {
        Logger.Log($"[EmptyAction] Detached from [{entity.root.name}]");
    }

    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTIme)
    {
        return false;
    }

    public void Execute(GameContext gameContext, Entity entity, float deltaTIme)
    {
    }
}