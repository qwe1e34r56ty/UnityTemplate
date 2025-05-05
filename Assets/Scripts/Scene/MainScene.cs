using UnityEngine;

public class MainScene : AScene
{
    public MainScene(string SceneID) : base(SceneID)
    {
    }
    public override void Build(GameContext gameContext)
    {
        base.Build(gameContext);
    }
    public override void Update()
    {

    }
    public override void Destroy(GameContext gameContext)
    {
        base.Destroy(gameContext);
    }
}