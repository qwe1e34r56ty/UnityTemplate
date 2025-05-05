using System.Collections.Generic;
using UnityEngine;

public class StartSceneLayoutInjectorStrategy : ALayoutInjectorStrategy
{
    public override void Inject(GameContext gameContext,
        Dictionary<string, GameObject> layout)
    {
        if (layout.TryGetValue(StartSceneLayoutElementID.Start, out var start))
        {
            StartInject(gameContext, start);
        }
        if (layout.TryGetValue(StartSceneLayoutElementID.Normalize, out var normalize))
        {
            NormalizeInject(gameContext, normalize);
        }
    }

    public void StartInject(GameContext gameContext, GameObject element)
    {
        SpriteRenderer spriteRenderer = element.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            var polygonCollider = element.AddComponent<PolygonCollider2D>();
            polygonCollider.isTrigger = true;
            ElementInject(gameContext,
                element,
                LeftClickAction: () =>
                {
                    gameContext.sceneCommandQueue.Enqueue(new ConvertSceneCommand(SceneID.Main, $"Convert to Scene {SceneID.Main} request"));
                },
                HoverEnterAction: () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[SpriteID.StartButtonHoverEnter];
                },
                HoverExitAction: () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[SpriteID.StartButton];
                });
            
        }
    }
    public void NormalizeInject(GameContext gameContext, GameObject element)
    {
        SpriteRenderer spriteRenderer = element.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            AnimationPlayer animationPlayer = new();
            animationPlayer.Play(element, gameContext.animationMap[AnimationID.Normalization], 0.1f, true);
            gameContext.updateHandlers.Add(animationPlayer);
        }
    }
}