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
        gameContext.sceneLayoutBindingMap = new();
        LoadSceneLayoutBindingMap(gameContext.sceneLayoutBindingMap);

        gameContext.layerNameSet = new();
        LoadLayerNameSet(gameContext.layerNameSet);
        foreach(string layerName in gameContext.layerNameSet)
        {
            if (!LayerUtility.IsValidLayerName(layerName))
            {
                Logger.LogError($"[GameContextInitializer] {layerName} Layer not found, please add Layer ");
            }
        }

        gameContext.tagNameSet = new();
        LoadTagNameSet(gameContext.tagNameSet);
        foreach (string tagName in gameContext.tagNameSet)
        {
            if (!TagUtility.IsValidTagName(tagName))
            {
                Logger.LogError($"[GameContextInitializer] {tagName} Tag not found, please add Tag ");
            }
        }

        gameContext.layoutDataMap = new();
        LoadLayoutMap(gameContext.layoutDataMap);

        gameContext.animationMap = new();
        LoadAnimationMap(gameContext.animationMap);
        gameContext.animationPlayerMap = new();

        gameContext.spriteMap = new();
        LoadSpriteMap(gameContext.spriteMap);

        gameContext.entityDataMap = new();
        LoadEntityMap(gameContext.entityDataMap);

        gameContext.sceneMap = new();
        RegisterScenes(gameContext.sceneMap);
    }

    private void RegisterScenes(Dictionary<string, AScene> sceneMap)
    {
        RegisterScene<StartScene>(sceneMap, SceneID.Start);
        RegisterScene<MainScene>(sceneMap, SceneID.Main);
    }

    private AScene RegisterScene<T>(Dictionary<string, AScene> sceneMap, string sceneID) where T : AScene
    {
        if (!sceneMap.ContainsKey(sceneID))
        {
            sceneMap[sceneID] = (AScene)Activator.CreateInstance(typeof(T), sceneID);
        }
        return sceneMap[sceneID];
    }

    private void LoadAnimationMap(Dictionary<string, (Sprite[], AnimationData)> animationMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.Animation);
        AnimationData[] animationDataArr = resourceManager.GetResource<AnimationData[]>(path);
        if (animationDataArr == null)
        {
            Logger.LogWarning("animationDataArr not found");
            return;
        }
        foreach (AnimationData animationData in animationDataArr)
        {
            Sprite[] frames = resourceManager.GetResource<Sprite[]>(animationData.path, animationData.pixelPerUnit);
            if(frames == null)
            {
                Logger.LogWarning($"frames not found : {animationData.path}");
                continue;
            }
            animationMap[animationData.id] = (frames, animationData);
        }
    }

    private void LoadLayerNameSet(HashSet<string> layerNameSet)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.LayerName);
        string[] layerNameArr = resourceManager.GetResource<string[]>(path);
        foreach(string layerName in layerNameArr)
        {
            layerNameSet.Add(layerName);
        }
        if (layerNameSet == null)
        {
            Logger.LogWarning("layerNameSet not found");
            return;
        }
    }

    private void LoadTagNameSet(HashSet<string> tagNameSet)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.TagName);
        string[] tagNameArr = resourceManager.GetResource<string[]>(path);
        foreach (string tagName in tagNameArr)
        {
            tagNameSet.Add(tagName);
        }
        if (tagNameSet == null)
        {
            Logger.LogWarning("TagNameSet not found");
            return;
        }
    }

    private void LoadSpriteMap(Dictionary<string, (Sprite, SpriteData)> spriteMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.Sprite);
        SpriteData[] spriteDataArr = resourceManager.GetResource<SpriteData[]>(path);
        if(spriteDataArr == null)
        {
            Logger.LogWarning("SpriteDataArr not found");
            return;
        }
        Texture2D texture2D;
        foreach (SpriteData spriteData in spriteDataArr)
        {
            path = Path.Combine(Application.streamingAssetsPath, spriteData.path);
            texture2D = resourceManager.GetResource<Texture2D>(path);
            if(texture2D == null)
            {
                Logger.LogWarning($"Texture File not found : {path}");
                continue;
            }
            spriteMap[spriteData.id] = (Sprite.Create(texture2D,
            new Rect(0, 0, texture2D.width, texture2D.height),
            Vector2.one * 0.5f,
            spriteData.pixelPerUnit), 
            spriteData);
        }
    }

    private void LoadSceneLayoutBindingMap(Dictionary<string, string[]> sceneLayoutBindingMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.SceneLayoutBinding);
        SceneLayoutBinding[] sceneLayoutBindingArr = resourceManager.GetResource<SceneLayoutBinding[]>(path);
        if (sceneLayoutBindingArr == null)
        {
            Logger.LogWarning("SceneLayoutBindingArr not found");
            return;
        }
        foreach (SceneLayoutBinding sceneLayoutBinding in sceneLayoutBindingArr)
        {
            sceneLayoutBindingMap[sceneLayoutBinding.sceneID] = sceneLayoutBinding.layoutID;
        }
    }

    private void LoadLayoutMap(Dictionary<string, LayoutData> layoutDataMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.Layout);
        LayoutPath[] layoutPathArr = resourceManager.GetResource<LayoutPath[]>(path);
        if (layoutPathArr == null)
        {
            Logger.LogWarning("LayoutPathArr not found");
            return;
        }
        LayoutData layoutData;
        foreach (LayoutPath layoutPath in layoutPathArr)
        {
            path = Path.Combine(Application.streamingAssetsPath, layoutPath.path);
            layoutData = resourceManager.GetResource<LayoutData>(path);
            if (layoutData == null)
            {
                Logger.LogWarning($"LayoutData not found : {layoutData.id}");
                continue;
            }
            layoutDataMap[layoutData.id] = layoutData;
        }
    }

    private void LoadEntityMap(Dictionary<string, EntityData> entityDataMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.Entity);
        EntityPath[] entityPathArr = resourceManager.GetResource<EntityPath[]>(path);
        if (entityPathArr == null)
        {
            Logger.LogWarning("EntityPathArr not found");
            return;
        }
        EntityData entityData;
        foreach (EntityPath entityPath in entityPathArr)
        {
            path = Path.Combine(Application.streamingAssetsPath, entityPath.path);
            entityData = resourceManager.GetResource<EntityData>(path);
            if (entityData == null)
            {
                Logger.LogWarning($"EntityData not found : {entityPath.id}");
                continue;
            }
            entityDataMap[entityPath.id] = entityData;
            Logger.Log(entityData.statArr[0].value.ToString());
        }
    }
}   