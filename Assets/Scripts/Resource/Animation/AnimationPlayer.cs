using UnityEngine;

public class AnimationPlayer : IUpdatable
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] frames;
    private float frameDuration;
    private bool loop;

    private int currentFrame = 0;
    private float timer = 0f;

    public void Play(GameObject target, Sprite[] frames, float frameDuration, bool loop)
    {
        this.spriteRenderer = target.GetComponent<SpriteRenderer>();
        this.frames = frames;
        this.frameDuration = frameDuration;
        this.loop = loop;
        this.currentFrame = 0;
        this.timer = 0f;

        if (frames == null)
        {
            Logger.LogWarning("frames not found");
        }
        spriteRenderer.sprite = frames[0];
    }

    public void Update(float deltaTime)
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