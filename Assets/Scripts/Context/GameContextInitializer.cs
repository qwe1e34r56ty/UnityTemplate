using System.Collections.Generic;
using System.IO;
using System.Resources;
using System;
using UnityEngine;

public class GameContextInitializer
{
    public ResourceManager resourceManager;
    public GameContextInitializer(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
    }
    public void InitGameContext(GameContext gameContext)
    {
        gameContext.camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        gameContext.sceneLayoutBindingMap = new();
        LoadSceneLayoutBindingMap(gameContext.sceneLayoutBindingMap);

        gameContext.layoutPathMap = new();
        LoadLayoutPathMap(gameContext.layoutPathMap);

        gameContext.layoutDataMap = new();
        LoadLayoutDataMap(gameContext.layoutDataMap,
            gameContext.layoutPathMap);

        gameContext.spriteMap = new();
        LoadSpriteMap(gameContext.spriteMap);

        gameContext.layerIDSet = new();
        LoadLayerIDSet(gameContext.layerIDSet);
        foreach(string layerID in gameContext.layerIDSet)
        {
            if(LayerMask.NameToLayer(layerID) == -1)
            {
                Debug.LogError($"{layerID} Layer not found, please add Layer ");
            }
        }

        gameContext.animationMap = new();
        LoadAnimationMap(gameContext.animationMap);

        gameContext.sceneMap = new();
        RegisterScenes(gameContext.sceneMap);
    }

    void RegisterScenes(Dictionary<string, AScene> sceneMap)
    {
        RegisterScene<StartScene>(sceneMap, SceneID.Start);
        RegisterScene<MainScene>(sceneMap, SceneID.Main);
        RegisterScene<EndScene>(sceneMap, SceneID.End);
    }

    AScene RegisterScene<T>(Dictionary<string, AScene> sceneMap, string sceneID) where T : AScene
    {
        if (!sceneMap.ContainsKey(sceneID))
        {
            sceneMap[sceneID] = (AScene)Activator.CreateInstance(typeof(T), sceneID);
        }
        return sceneMap[sceneID];
    }

    void LoadAnimationMap(Dictionary<string, Sprite[]> animationMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.Animation);
        AnimationData[] animationDataArr = resourceManager.GetResource<AnimationData[]>(path);
        if (animationDataArr == null)
        {
            Debug.LogWarning("animationDataArr not found");
            return;
        }
        foreach (AnimationData animationData in animationDataArr)
        {
            Sprite[] frames = resourceManager.GetResource<Sprite[]>(animationData.path, animationData.pixelPerUnit);
            if(frames == null)
            {
                Debug.LogWarning($"frames not found : {animationData.path}");
                continue;
            }
            animationMap[animationData.id] = frames;
        }
    }

    void LoadLayerIDSet(HashSet<string> layerIDSet)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.LayerID);
        string[] layerIDArr = resourceManager.GetResource<string[]>(path);
        foreach(string layerID in layerIDArr)
        {
            layerIDSet.Add(layerID);
        }
        if (layerIDSet == null)
        {
            Debug.LogWarning("layerIDSet not found");
            return;
        }
    }

    void LoadSpriteMap(Dictionary<string, Sprite> spriteMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.Sprite);
        SpriteData[] spriteDataArr = resourceManager.GetResource<SpriteData[]>(path);
        if(spriteDataArr == null)
        {
            Debug.LogWarning("SpritePathArr not found");
            return;
        }
        Texture2D texture2D;
        foreach (SpriteData spriteData in spriteDataArr)
        {
            path = Path.Combine(Application.streamingAssetsPath, spriteData.path);
            texture2D = resourceManager.GetResource<Texture2D>(path);
            if(texture2D == null)
            {
                Debug.LogWarning($"Texture File not found : {path}");
                continue;
            }
            spriteMap[spriteData.id] = Sprite.Create(texture2D,
            new Rect(0, 0, texture2D.width, texture2D.height),
            Vector2.one * 0.5f,
            spriteData.pixelPerUnit);
        }
    }

    void LoadSceneLayoutBindingMap(Dictionary<string, string[]> sceneLayoutBindingMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.SceneLayoutBinding);
        SceneLayoutBinding[] sceneLayoutBindingArr = resourceManager.GetResource<SceneLayoutBinding[]>(path);
        if (sceneLayoutBindingArr == null)
        {
            Debug.LogWarning("SceneLayoutBindingArr not found");
            return;
        }
        foreach (SceneLayoutBinding sceneLayoutBinding in sceneLayoutBindingArr)
        {
            sceneLayoutBindingMap[sceneLayoutBinding.sceneID] = sceneLayoutBinding.layoutID;
        }
    }

    void LoadLayoutPathMap(Dictionary<string, string> layoutPathMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.LayoutPath);
        LayoutPath[] layoutPathArr = resourceManager.GetResource<LayoutPath[]>(path);
        if (layoutPathArr == null)
        {
            Debug.LogWarning("LayoutPathArr not found");
            return;
        }
        foreach (LayoutPath layoutPath in layoutPathArr)
        {
            layoutPathMap[layoutPath.id] = layoutPath.path;
        }
    }
    void LoadLayoutDataMap(Dictionary<string, LayoutData> layoutDataMap,
        Dictionary<string, string> layoutPathMap)
    {
        string path;
        LayoutData layoutData;
        foreach (var pair in layoutPathMap)
        {
            path = Path.Combine(Application.streamingAssetsPath, pair.Value);
            layoutData = resourceManager.GetResource<LayoutData>(path);
            if (layoutData == null)
            {
                Debug.LogWarning($"LayoutData not found : {pair.Key}");
                continue;
            }
            layoutDataMap[pair.Key] = layoutData;
        }
    }
}