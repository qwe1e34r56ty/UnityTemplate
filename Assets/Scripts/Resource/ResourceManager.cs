using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager
{
    private readonly Dictionary<string, object> resourceCache = new();
    private static class ResourceLoader
    {
        private readonly static Dictionary<Type, object> resourceLoadStrategies = new Dictionary<Type, object>
        {
            { typeof(Texture2D), new TextureLoadStrategy() },
            { typeof(string), new TextLoadStrategy() },
            { typeof(string[]) , new JsonArrayLoadStrategy<string>() },
            { typeof(EntityPath[]), new JsonArrayLoadStrategy<EntityPath>() },
            { typeof(EntityData), new JsonLoadStrategy<EntityData>() },
            { typeof(ScenePath[]), new JsonArrayLoadStrategy<ScenePath>() },
            { typeof(SceneData), new JsonLoadStrategy<SceneData>() },
            { typeof(AnimationPath[]), new JsonArrayLoadStrategy<AnimationPath>() },
            { typeof(Sprite[]), new AnimationLoadStrategy() }
        };

        private static class StrategyCache<T>
        {
            public static readonly IResourceLoadStrategy<T> cachedStrategy = null;

            static StrategyCache()
            {
                Type type = typeof(T);
                if (resourceLoadStrategies.TryGetValue(type, out var strategy))
                {
                    if (strategy is IResourceLoadStrategy<T> _strategy)
                    {
                        cachedStrategy = _strategy;
                    }
                    else
                    {
                        Logger.LogError($"[ResourceManager] Resource Load Strategy for {type} mismatched for IResourceLoaderStrategy<{type}>");
                    }
                }
                else
                {
                    Logger.LogError($"[ResourceManager] Resource Load Strategy not found: {type}");
                }
            }
        }
        public static T Load<T>(string path, int pixelPerUnit = 100)
        {
            if (StrategyCache<T>.cachedStrategy != null)
            {
                return StrategyCache<T>.cachedStrategy.Load(path, pixelPerUnit);
            }
            return default;
        }
    }

    public T GetResource<T>(string relativePath, int pixelPerUnit = 100)
    {
        if (resourceCache.ContainsKey(relativePath))
            return (T)resourceCache[relativePath];
        T resource = ResourceLoader.Load<T>(relativePath, pixelPerUnit);
        resourceCache[relativePath] = resource;
        return resource;
    }
    public void LoadResource<T>(string relativePath, int pixelPerUnit = 100)
    {
        if (resourceCache.ContainsKey(relativePath))
            return;
        T resource = ResourceLoader.Load<T>(relativePath, pixelPerUnit);
        resourceCache[relativePath] = resource;
    }
    public void UnLoadResource(string relativePath)
    {
        if (resourceCache.ContainsKey(relativePath))
        {
            resourceCache.Remove(relativePath);
        }
    }
    public void ClearResources()
    {
        resourceCache.Clear();
    }
}
