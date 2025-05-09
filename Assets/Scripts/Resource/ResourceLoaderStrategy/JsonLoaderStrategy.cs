using UnityEngine;
using System.IO;

public class JsonLoaderStrategy<T> : IResourceLoaderStrategy<T> where T : class
{
    public T Load(string path, int pixelPerUnit = 100)
    {
        if (!File.Exists(path))
        {
            Logger.LogError($"JSON file not found: {path}");
            return null;    
        }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}