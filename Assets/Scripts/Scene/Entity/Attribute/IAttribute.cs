public interface IAttribute
{
    public void Attach(Entity entity, GameContext gameContext);
    public void Detach(Entity entity, GameContext gameContext);
}