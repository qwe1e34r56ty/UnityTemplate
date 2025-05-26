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

    public T Load<T>(string path, int pixelPerUnit = 100)
    {
        if(resourceLoaderStrategies.ContainsKey(typeof(T)))
        {
            var strategy = resourceLoaderStrategies[typeof(T)];
            if (strategy is IResourceLoaderStrategy<T>)
            {
                return ((IResourceLoaderStrategy<T>)strategy).Load(path, pixelPerUnit);
            }
            else
            {
                Logger.LogError($"[ResoureLoader] Resource Load Strategy for {typeof(T)} not match for IResourceLoaderStrategy<{typeof(T)}>");
                return default(T);
            }
        }
        else
        {
            Logger.LogError($"[ResoureLoader] Resource Load Strategy not found : {typeof(T)}");
            return default(T);
        }
    }
}
