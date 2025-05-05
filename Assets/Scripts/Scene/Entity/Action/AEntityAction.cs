public abstract class AEntityAction
{
    public int priority { get; private set; }
    public abstract bool CanExecute(Entity entity, GameContext context);
    public abstract void Execute(Entity entity, GameContext context);
}