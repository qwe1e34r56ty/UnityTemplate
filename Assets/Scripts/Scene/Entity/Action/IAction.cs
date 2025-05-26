using static UnityEngine.EventSystems.EventTrigger;

public interface IAction
{
    // Action을 Entity에 붙일 때 발생해야 하는 동작
    public void Attach(GameContext gameContext, Entity entity, int priority);

    // Action을 Entity에서 떼어낼 때 발생해야 하는 동작
    public void Detach(GameContext gameContext, Entity entity);

    // Execute 가능 여부 return
    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTime);

    // UpdateableAction 붙어있는 Entity에서 동작
    public void Execute(GameContext gameContext, Entity entity, float deltaTime);
}