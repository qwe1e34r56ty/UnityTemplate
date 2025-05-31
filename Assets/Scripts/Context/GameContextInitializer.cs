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
        gameContext.sceneDataMap = new();
        LoadSceneMap(gameContext.sceneDataMap);

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

        gameContext.entityDataMap = new();
        LoadEntityMap(gameContext.entityDataMap);

        gameContext.animationDataMap = new();
        LoadAnimationMap(gameContext.animationDataMap);
        gameContext.animationPlayerMap = new();

        gameContext.sceneMap = new();
        RegisterScenes(gameContext.sceneMap);
    }

    private void RegisterAction(Dictionary<string, IAction> actionMap)
    {

    }

    private void RegisterScenes(Dictionary<string, AScene> sceneMap)
    {
        RegisterScene<StartScene>(sceneMap, SceneID.Start);
        RegisterScene<MainScene>(sceneMap, SceneID.Main);
    }

    private AScene RegisterScene<T>(Dictionary<string, AScene> sceneMap, string id) where T : AScene
    {
        if (!sceneMap.ContainsKey(id))
        {
            sceneMap[id] = (AScene)Activator.CreateInstance(typeof(T), id);
        }
        return sceneMap[id];
    }

    private void LoadAnimationMap(Dictionary<string, (Sprite[], AnimationPath)> animationDataMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.Animation);
        AnimationPath[] animationDataArr = resourceManager.GetResource<AnimationPath[]>(path);
        if (animationDataArr == null)
        {
            Logger.LogWarning("animationDataArr not found");
            return;
        }
        foreach (AnimationPath animationData in animationDataArr)
        {
            Sprite[] frames = resourceManager.GetResource<Sprite[]>(animationData.path, animationData.pixelPerUnit);
            if(frames == null)
            {
                Logger.LogWarning($"frames not found : {animationData.path}");
                continue;
            }
            animationDataMap[animationData.id] = (frames, animationData);
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

    private void LoadSceneMap(Dictionary<string, SceneData> sceneDataMap)
    {
        string path = Path.Combine(Application.streamingAssetsPath, JsonPath.Scene);
        ScenePath[] scenePathArr = resourceManager.GetResource<ScenePath[]>(path);
        if(scenePathArr == null)
        {
            Logger.LogWarning("ScenePathArr not found");
            return;
        }
        SceneData sceneData;
        foreach (ScenePath scenePath in scenePathArr)
        {
            path = Path.Combine(Application.streamingAssetsPath, scenePath.path);
            sceneData = resourceManager.GetResource<SceneData>(path);
            if (sceneData == null)
            {
                Logger.LogWarning("SceneDataArr not found");
                return;
            }
            sceneDataMap[sceneData.id] = sceneData;
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
                Logger.LogWarning($"EntityData not found : {entityData.id}");
                continue;
            }
            entityDataMap[entityData.id] = entityData;
        }
    }
}   