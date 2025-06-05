using System.Xml.Linq;
using UnityEngine;

public class AnimationPlayer : IUpdateable
{
    private SpriteRenderer spriteRenderer;
    private GameObject gameObject;
    private Sprite[] frames;
    private float frameDuration;
    private bool loop;

    private int currentFrame = 0;
    private float timer = 0f;

    public void Play(GameContext gameContext, GameObject target,
        (Sprite[], AnimationPath) animationData)
    {
        gameObject = target;
        Sprite[] frames = animationData.Item1;
        if (frames == null)
        {
            Logger.LogWarning("[AnimationPlayer] frames not found");
            return;
        }

        AnimationPath metaData = animationData.Item2;
        this.frames = frames;
        this.frameDuration = metaData.frameDuration;
        this.loop = metaData.loop;
        this.currentFrame = 0;
        this.timer = 0f;

        this.spriteRenderer = target.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = this.frames[this.currentFrame];
        if (!gameContext.updateHandlers.Contains(this))
        {
            gameContext.updateHandlers.Add(this);
        }
    }

    public void Pause(GameContext gameContext, GameObject target)
    {
        if (gameContext.updateHandlers.Contains(this))
        {
            gameContext.updateHandlers.Remove(this);
        }
    }

    public void Update(GameContext gameContext, float deltaTime)
    {
        if (spriteRenderer == null || frames == null || frames.Length == 0) return;

        timer += deltaTime;
        if (timer >= frameDuration)
        {
            timer -= frameDuration;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (loop)
                    currentFrame = 0;
                else
                    currentFrame = frames.Length - 1;
            }
            spriteRenderer.sprite = frames[currentFrame];
        }
    }
}