using System.Collections.Generic;
using UnityEngine;

public class StartLayoutInjectorStrategy : ILayoutInjectorStrategy
{
    public void Inject(GameContext gameContext,
        Dictionary<string, GameObject> layout)
    {
        if (layout.ContainsKey(StartSceneLayoutElementID.Start))
        {
            GameObject start = layout[StartSceneLayoutElementID.Start];
            SpriteRenderer spriteRenderer = start.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var polygonCollider = start.AddComponent<PolygonCollider2D>();
                polygonCollider.isTrigger = true;
                gameContext.onLeftClickHandlers[start] = () =>
                {
                    Debug.Log("정 상 화");
                };
                // 예시
                gameContext.onHoverEnterHandlers[start] = () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[StartSceneLayoutSpriteID.StartHoverEnter];
                };
                gameContext.onHoverExitHandlers[start] = () =>
                {
                    spriteRenderer.sprite = gameContext.spriteMap[StartSceneLayoutSpriteID.Start];
                };
            }
            else
            {
                Debug.LogError($"SpriteRenderer not found : {start.name}");
            }
        }
    }
}