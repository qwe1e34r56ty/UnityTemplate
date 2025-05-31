using UnityEngine;
using System.IO;

public class TextureLoaderStrategy : IResourceLoaderStrategy<Texture2D>
{
    public Texture2D Load(string path, int pixelPerUnit = 100)
    {
        if (!File.Exists(path))
        {
            Logger.LogError($"Texture file not found: {path}");
            return null;
        }          
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D texture2D = new Texture2D(2, 2);
        texture2D.LoadImage(bytes);
        return texture2D;
    }
}