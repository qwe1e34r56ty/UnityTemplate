using System.IO;
using UnityEngine;

public class SaveManager
{
    private readonly string saveDirectory = Path.Combine(Application.streamingAssetsPath, "Saves");

    private const string defaultFileName = "save.json";

    public void Save(GameContext context,
        string saveFileName = defaultFileName)
    {
        string path = Path.Combine(saveDirectory, saveFileName);
        SaveData data = new();
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
        Logger.Log($"Save complete: {path}");
    }

    public SaveData Load(string saveFileName = defaultFileName)
    {
        string path = Path.Combine(saveDirectory, saveFileName);
        if (!File.Exists(path))
        {
            Logger.Log("Save not found");
            return null;
        }
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Logger.Log("Save Loading Complete");
        return data;
    }
}
