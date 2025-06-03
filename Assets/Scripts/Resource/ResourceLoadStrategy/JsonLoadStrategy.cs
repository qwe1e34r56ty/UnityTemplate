using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JsonLoadStrategy<T> : IResourceLoadStrategy<T> where T : class
{
    public T Load(string path, int pixelPerUnit = 100)
    {
        if (!File.Exists(path))
        {
            Logger.LogError($"JSON file not found: {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        if (string.IsNullOrWhiteSpace(json) || !json.TrimStart().StartsWith("{"))
        {
            Logger.LogError($"Invalid or empty JSON at: {path}");
            return null;
        }

        T result = JsonConvert.DeserializeObject<T>(json);
        if (result == null)
        {
            Logger.LogError($"Deserialization failed: {typeof(T).Name} is null. Path: {path}");
        }

        return result;
    }
}
