using UnityEngine;

public class MainScene : AScene
{
    GameObject player;
    GameObject map;
    public MainScene(string SceneID) : base(SceneID)
    {
    }
    public override void Build(GameContext gameContext)
    {
        base.Build(gameContext);
        player = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        map = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Map"));
    }
    public override void Update(float deltaTime)
    {

    }
    public override void Destroy(GameContext gameContext)
    {
        base.Destroy(gameContext);
        GameObject.Destroy(player);
        GameObject.Destroy(map);
    }
}