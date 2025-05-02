using UnityEngine;
using System.IO;

public class JsonArrayLoaderStrategy<T> : IResourceLoaderStrategy<T[]> where T : class
{
    public T[] Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"JSON file not found: {path}");
            return null;
        }
        string json = File.ReadAllText(path);
        return JsonHelper.FromJson<T>(json);
    }
}