using UnityEngine;

public class SceneConverter
{
    public void ConvertScene(GameContext gameContext, string SceneID)
    {
        AScene currentScene = gameContext.currentScene;
        if (!gameContext.sceneMap.TryGetValue(SceneID, out var nextScene))
        {
            Debug.Log($"Next Scene not found : {SceneID}");
            return;
        }
        if (currentScene != null)
        {
            currentScene.Destroy(gameContext);
        }   
        nextScene.Build(gameContext);
        gameContext.currentScene = nextScene;
        if (gameContext.camera != null)
        {
            gameContext.camera.transform.position = new Vector3(0f, 0f, -10f);
        }
    }
}