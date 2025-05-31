using UnityEngine;

public class SceneConverter
{
    public void ConvertScene(GameContext gameContext, string nextSceneID)
    {
        Scene scene = gameContext.scene;
        if (!gameContext.sceneDataMap.ContainsKey(nextSceneID))
        {
            Logger.Log($"Next Scene not found : {nextSceneID}");
            return;
        }
        if (scene != null)
        {
            scene.Clear(gameContext);
        }
        scene.Build(gameContext, nextSceneID);
    }
}