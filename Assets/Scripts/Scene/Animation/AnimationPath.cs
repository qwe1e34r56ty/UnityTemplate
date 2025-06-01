using System;

[Serializable]
public class AnimationPath
{
    public string id;
    public string path;
    public int pixelPerUnit;
    public float frameDuration;
    public bool loop;
}