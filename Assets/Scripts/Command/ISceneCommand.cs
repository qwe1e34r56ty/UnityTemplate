public interface ISceneCommand
{
    void Execute(GameContext gameContext, SceneBuilder builder);
}