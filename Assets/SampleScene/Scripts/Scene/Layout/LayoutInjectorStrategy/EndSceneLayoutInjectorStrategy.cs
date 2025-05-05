using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class EndSceneLayoutInjectorStrategy : ALayoutInjectorStrategy
{
    public override void Inject(GameContext gameContext,
        Dictionary<string, GameObject> layout)
    {
        if (layout.TryGetValue(EndSceneLayoutElementID.ToTitle, out var element))
        {
            ToTitleInject(gameContext, element);
        }
    }

    public void ToTitleInject(GameContext gameContext, GameObject element)
    {
        SpriteRenderer spriteRenderer = element.GetComponent<SpriteRenderer>();
        var collider = element.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
        if (spriteRenderer != null)
        {
            ElementInject(gameContext,
                element,
                LeftClickAction: () =>
                {
                    gameContext.sceneCommandQueue.Enqueue(
                        new ConvertSceneCommand(SceneID.Start,
                        $"Convert to Scene {SceneID.Start} request"));
                },
                HoverEnterAction: () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[SpriteID.ToTitleHoverEnter];
                },
                HoverExitAction: () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[SpriteID.ToTitle];
                });
        }
    }
}