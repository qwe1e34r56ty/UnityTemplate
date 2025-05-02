using UnityEngine;
using System.IO;

public class TextLoaderStrategy : IResourceLoaderStrategy<string>
{
    public string Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"JSON file not found: {path}");
            return null;
        }
        string text = File.ReadAllText(path);
        return text;
    }
}