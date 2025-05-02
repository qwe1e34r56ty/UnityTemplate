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
        resourceLoaderStrategies[typeof(LayoutData)] = new JsonLoaderStrategy<LayoutData>();
        resourceLoaderStrategies[typeof(LayoutPath[])] = new JsonArrayLoaderStrategy<LayoutPath>();
    }

    public T Load<T>(string path)
    {
        if(resourceLoaderStrategies.ContainsKey(typeof(T)))
        {
            var strategy = resourceLoaderStrategies[typeof(T)];
            if (strategy is IResourceLoaderStrategy<T>)
            {
                return ((IResourceLoaderStrategy<T>)strategy).Load(path);
            }
            else
            {
                Debug.LogError($"Strategy for {typeof(T)} not match for IResourceLoaderStrategy<{typeof(T)}>");
                return default(T);
            }
        }
        else
        {
            Debug.LogError($"Strategy not found : {typeof(T)}");
            return default(T);
        }
    }
}
