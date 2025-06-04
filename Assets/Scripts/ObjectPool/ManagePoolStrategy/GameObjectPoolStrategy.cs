using Newtonsoft.Json.Serialization;
using UnityEngine;

public class GameObjectPoolStrategy : AutoResizablePoolStrategy<GameObject>
{
    public GameObjectPoolStrategy(GameContext gameContext, int initialCapacity) : base(gameContext, initialCapacity)
    {
    }

    protected override void OnGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    protected override void OnReturn(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(null);
    }

    protected override void OnDequeue(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    protected override GameObject CreateInstance()
    {
        GameObject gameObject = new GameObject();
        gameObject.SetActive(false);
        return gameObject;
    }
}