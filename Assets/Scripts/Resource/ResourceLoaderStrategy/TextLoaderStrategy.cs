using UnityEngine;
using System.IO;

public class TextLoaderStrategy : IResourceLoaderStrategy<string>
{
    public string Load(string path, int pixelPerUnit = 100)
    {
        if (!File.Exists(path))
        {
            Logger.LogError($"JSON file not found: {path}");
            return null;
        }
        string text = File.ReadAllText(path);
        return text;
    }
}