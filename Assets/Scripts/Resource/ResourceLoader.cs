using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceLoader
{
    private readonly Dictionary<Type, object> resourceLoaderStrategies = new Dictionary<Type, object>();

    public ResourceLoader()
    {
        resourceLoaderStrategies[typeof(Texture2D)] = new TextureLoaderStrategy();
        resourceLoaderStrategies[typeof(string)] = new TextLoaderStrategy();
        resourceLoaderStrategies[typeof(string[])] = new JsonArrayLoaderStrategy<string>();

        resourceLoaderStrategies[typeof(EntityPath[])] = new JsonArrayLoaderStrategy<EntityPath>();
        resourceLoaderStrategies[typeof(EntityData)] = new JsonLoaderStrategy<EntityData>();

        resourceLoaderStrategies[typeof(ScenePath[])] = new JsonArrayLoaderStrategy<ScenePath>();
        resourceLoaderStrategies[typeof(SceneData)] = new JsonLoaderStrategy<SceneData>();

        resourceLoaderStrategies[typeof(AnimationPath[])] = new JsonArrayLoaderStrategy<AnimationPath>();
        resourceLoaderStrategies[typeof(Sprite[])] = new AnimationLoaderStrategy();
    }

    private static class StrategyCache<T>
    {
        public static IResourceLoaderStrategy<T>? cachedStrategy = null;
    }

    public T Load<T>(string path, int pixelPerUnit = 100)
    {
        if (StrategyCache<T>.cachedStrategy != null)
        {
            return StrategyCache<T>.cachedStrategy.Load(path, pixelPerUnit);
        }
        Type type = typeof(T);
        if (resourceLoaderStrategies.TryGetValue(type, out object strategy))
        {
            if (strategy is IResourceLoaderStrategy<T> _strategy)
            {
                StrategyCache<T>.cachedStrategy = _strategy;
                return _strategy.Load(path, pixelPerUnit);
            }
            else
            {
                Logger.LogError($"[ResourceLoader] Resource Load Strategy for {type} mismatched for IResourceLoaderStrategy<{type}>");
                return default;
            }
        }

        Logger.LogError($"[ResourceLoader] Resource Load Strategy not found: {type}");
        return default;
    }
}
