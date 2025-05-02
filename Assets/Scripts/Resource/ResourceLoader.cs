using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceLoader
{
    private readonly Dictionary<Type, object> resourceLoadStrategy = new Dictionary<Type, object>();

    public ResourceLoader()
    {
        resourceLoadStrategy[typeof(Texture2D)] = new TextureLoaderStrategy();
        resourceLoadStrategy[typeof(string)] = new TextLoaderStrategy();
        resourceLoadStrategy[typeof(LayoutData)] = new JsonLoaderStrategy<LayoutData>();
        resourceLoadStrategy[typeof(LayoutPath[])] = new JsonArrayLoaderStrategy<LayoutPath>();
    }

    public T Load<T>(string path)
    {
        if(resourceLoadStrategy.TryGetValue(typeof(T), out var strategyValue))
        {
            if((strategyValue) is IResourceLoaderStrategy<T> strategy)
            {
                return strategy.Load(path);
            }else {
                Debug.LogError($"Strategy for {typeof(T)} not match for IResouuceLoader<{typeof(T)}>");
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
