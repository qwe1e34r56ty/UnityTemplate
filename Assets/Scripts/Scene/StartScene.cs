using UnityEngine;

public class StartScene : AScene
{
    public StartScene(string SceneID) : base(SceneID)
    {
    }
    public override void Build(GameContext gameContext)
    {
        base.Build(gameContext);
    }
    public override void Update(float deltaTime)
    {

    }
    public override void Destroy(GameContext gameContext)
    {
        base.Destroy(gameContext);
    }
}