using System.Collections.Generic;
using UnityEngine;

public class StartSceneLayoutInjectorStrategy : ALayoutInjectorStrategy
{
    public override void Inject(GameContext gameContext,
        Dictionary<string, GameObject> layout)
    {
        if (layout.ContainsKey(StartSceneLayoutElementID.Start))
        {
            if (layout.TryGetValue(StartSceneLayoutElementID.Start, out var start))
            {
                StartInject(gameContext, start);
            }
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
}