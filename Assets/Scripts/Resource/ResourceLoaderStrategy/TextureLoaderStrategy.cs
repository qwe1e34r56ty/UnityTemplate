using UnityEngine;
using System.IO;

public class TextureLoaderStrategy : IResourceLoaderStrategy<Texture2D>
{
    public Texture2D Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"Texture file not found: {path}");
            return null;
        }          
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        return texture;
    }
}