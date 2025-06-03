using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimationLoadStrategy : IResourceLoadStrategy<Sprite[]>
{
    public Sprite[] Load(string path, int pixelPerUnit = 100)
    {
        string directoryPath = Path.Combine(Application.streamingAssetsPath, path);
        string[] files = Directory.GetFiles(directoryPath, "*.png");
        List<Sprite> frames = new();

        foreach (string file in files)
        {
            byte[] data = File.ReadAllBytes(file);
            Texture2D texture2D = new(2, 2);
            texture2D.LoadImage(data);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), pixelPerUnit);
            frames.Add(sprite);
        }

        return frames.ToArray();
    }
}