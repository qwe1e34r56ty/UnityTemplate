using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneLayoutInjectorStrategy : ALayoutInjectorStrategy
{
    public override void Inject(GameContext gameContext,
        Dictionary<string, GameObject> layout)
    {
        if (layout.TryGetValue(MainSceneLayoutElementID.Background, out var background))
        {
            ToBackgroundInject(gameContext, background);
        }
        if (layout.TryGetValue(MainSceneLayoutElementID.ToTitle, out var toTitle))
        {
            ToTitleInject(gameContext, toTitle);
        }
        if (layout.TryGetValue(MainSceneLayoutElementID.ToEnd, out var toEnd))
        {
            ToEndInject(gameContext, toEnd);
        }
    }

    public void ToBackgroundInject(GameContext gameContext, GameObject element)
    {
        SpriteRenderer spriteRenderer = element.GetComponent<SpriteRenderer>();
        var collider = element.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        if (spriteRenderer != null)
        {
            ElementInject(gameContext,
                element,
                RightClickAction: () =>
                {
                    Logger.Log("Right Click!!");
                    //gameContext.ClearBeforeLoadScene();
                    //SceneManager.LoadScene("MainScene");
                });
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
                    spriteRenderer.sprite = gameContext.spriteMap[SpriteID.ToTitleButtonHoverEnter];
                },
                HoverExitAction: () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[SpriteID.ToTitleButton];
                });
        }
    }

    public void ToEndInject(GameContext gameContext, GameObject element)
    {
        SpriteRenderer spriteRenderer = element.GetComponent<SpriteRenderer>();
        var collider = element.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        if (spriteRenderer != null)
        {
            ElementInject(gameContext,
                element,
                LeftClickAction: () =>
                {
                    gameContext.sceneCommandQueue.Enqueue(new ConvertSceneCommand(SceneID.End, $"Convert to Scene {SceneID.End} request"));
                },
                HoverEnterAction: () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[SpriteID.ToEndButtonHoverEnter];
                },
                HoverExitAction: () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[SpriteID.ToEndButton];
                });
        }
    }
}