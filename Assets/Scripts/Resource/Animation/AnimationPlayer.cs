using System.Xml.Linq;
using UnityEngine;

public class AnimationPlayer : IUpdatable
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] frames;
    private float frameDuration;
    private bool loop;

    private int currentFrame = 0;
    private float timer = 0f;

    public void Play(GameObject target,
        (Sprite[], AnimationData) animationData,
        Vector3? offsetPosition = null,
        Vector3? offsetRotation = null,
        Vector3? offsetScale = null,
        int? offsetSortingOrder = null)
    {
        Sprite[] frames = animationData.Item1;
        if (frames == null)
        {
            Logger.LogWarning("[AnimationPlayer] frames not found");
            return;
        }

        AnimationData metaData = animationData.Item2;
        this.frames = frames;
        this.frameDuration = metaData.frameDuration;
        this.loop = metaData.loop;
        this.currentFrame = 0;
        this.timer = 0f;

        this.spriteRenderer = target.GetComponent<SpriteRenderer>();
        this.spriteRenderer.sortingOrder += offsetSortingOrder ?? 0;
        Transform transform = target.transform;
        transform.position += (offsetPosition ?? Vector3.zero);
        transform.localScale = Vector3.Scale(transform.localScale, offsetScale ?? Vector3.one);
        transform.rotation = offsetRotation.HasValue ? Quaternion.Euler(offsetRotation.Value) : Quaternion.identity;
        spriteRenderer.sprite = this.frames[this.currentFrame];
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