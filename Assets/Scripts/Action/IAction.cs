using static UnityEngine.EventSystems.EventTrigger;

public interface IAction
{
    public void Attach(GameContext gameContext, Entity entity, int priority);
    
    public void Detach(GameContext gameContext, Entity entity);
    
    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTime);
    
    public void Execute(GameContext gameContext, Entity entity, float deltaTime);
}