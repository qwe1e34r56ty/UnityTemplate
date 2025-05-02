using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager
{
    private readonly Dictionary<string, object> resourceCache = new();
    private readonly ResourceLoader resourceLoader = new();

    public T GetResource<T>(string relativePath)
    {
        if (resourceCache.ContainsKey(relativePath))
            return (T)resourceCache[relativePath];
        T resource = resourceLoader.Load<T>(relativePath);
        resourceCache[relativePath] = resource;
        return resource;
    }

    public void LoadResource<T>(string relativePath)
    {
        if (resourceCache.ContainsKey(relativePath))
            return;
        T resource = resourceLoader.Load<T>(relativePath);
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
