using static UnityEngine.EventSystems.EventTrigger;

public interface IAction
{
    // Action�� Entity�� ���� �� �߻��ؾ� �ϴ� ����
    public void Attach(GameContext gameContext, Entity entity, int priority);

    // Action�� Entity���� ��� �� �߻��ؾ� �ϴ� ����
    public void Detach(GameContext gameContext, Entity entity);

    // Execute ���� ���� return
    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTime);

    // UpdateableAction �پ��ִ� Entity���� ����
    public void Execute(GameContext gameContext, Entity entity, float deltaTime);
}